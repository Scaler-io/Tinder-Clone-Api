using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System.Threading.Tasks;
using AutoMapper;
using Tinder_Dating_API.DataAccess.Interfaces;
using Tinder_Dating_API.DataAccess.Specifications.User;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Infrastructure;
using Tinder_Dating_API.Models.Constants;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Requests.Photo;
using Tinder_Dating_API.Models.Responses;
using System.IO;
using System.Linq;
using System;
using Tinder_Dating_API.Services.Identity;

namespace Tinder_Dating_API.Services.MemberImage
{
    public class ImageService : IImageService
    {
        private readonly ILogger _logger;
        private readonly Cloudinary _cloudinary;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public ImageService(
            ILogger logger,
            IOptions<CloudinaryOption> cloudinaryConfig,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            IIdentityService identityService)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            var acc = new Account(
                cloudinaryConfig.Value.CloudName,
                cloudinaryConfig.Value.ApiKey,
                cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
            _identityService = identityService;
        }

        public async Task<Result<MemberImageResponse>> AddImageAsync(UploadPhotoRequest request) 
        {
            _logger.Here().MethoEnterd();

            var user = await _identityService.GetCurrentAuthUser();

            var file = request.File;
            var uploadResult = new ImageUploadResult();

            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = PrepareImageUploadParams(file, stream);
                uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    _logger.Error($"{ErrorCodes.Operationfailed}: Image upload to cloudinary failed. {uploadResult.Error.Message}");
                    return Result<MemberImageResponse>.Fail(ErrorCodes.Operationfailed);
                }

                _logger.Here().Information("Image upload to cloudinary success. {@publicId}", uploadParams.PublicId);
            }
           
            _logger.Here().MethodExited();
            return await PrepareImageResposne(uploadResult, user);
        }

        public async Task<Result<bool>> DeleteImageAsync(DeletePhotoRequest request)
        {
            _logger.Here().MethoEnterd();

            var user = await _identityService.GetCurrentAuthUser();

            var photo = user.Profile.Images.FirstOrDefault(x => x.Id == Guid.Parse(request.PhotoId));

            if(photo == null)
            {
                _logger.Here().Information("No photo was found with id {@PhotoId}.", request.PhotoId);
                return Result<bool>.Fail(ErrorCodes.NotFound);
            }

            if (photo.IsMain)
            {
                _logger.Here().Information($"{ErrorCodes.BadRequest}: Trying to delete main photo.");
                return Result<bool>.Fail(ErrorCodes.BadRequest, "Trying to delete main photo.");
            }

            if(photo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photo.PublicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);

                if (result.Error != null)
                {
                    _logger.Error($"{ErrorCodes.Operationfailed}: Image deletion failed");
                    return Result<bool>.Fail(ErrorCodes.Operationfailed, result.Error.Message);
                }
            }

            user.Profile.Images.Remove(photo);

            if(await _unitOfWork.Complete() < 1)
            {
                _logger.Here().Information("Photo deleteion from database failed.");
                return Result<bool>.Fail(ErrorCodes.Operationfailed, "Photo deletion from database failed.");
            }

            _logger.Here().Information("Image deleted successfully.");
            _logger.Here().MethodExited();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdatePhotoAsMain(UpdatePhotoRequest request)
        {
            _logger.Here().MethoEnterd();

            var user = await _identityService.GetCurrentAuthUser();

            var photo = user.Profile.Images.FirstOrDefault(x => x.Id == Guid.Parse(request.PhotoId));

            if (photo.IsMain)
            {
                _logger.Here().Information("The image is alredy set as main.");
                return Result<bool>.Fail(ErrorCodes.BadRequest, "The image is alredy set as main.");
            }

            var currentMain = user.Profile.Images.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _unitOfWork.Complete() < 1)
            {
                _logger.Here().Information("{@OperationFailed}: Falied to update photo with id. {@photoId}",
                    ErrorCodes.Operationfailed,request.PhotoId);
                return Result<bool>.Fail(ErrorCodes.Operationfailed, "Failed to update photo.");
            }

            _logger.Here().MethodExited();

            _logger.Here().Information("The photo has been set as main. {@photoId}", request.PhotoId);
            return Result<bool>.Success(true);
        }

        private ImageUploadParams PrepareImageUploadParams(IFormFile file, Stream stream)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation()
                                        .Height(500)
                                        .Width(500)
                                        .Crop("fill")
                                        .Gravity("face")
            };

            _logger.Here().Information("Image params prepared for uploading.");
            return uploadParams;
        }

        private async Task<Result<MemberImageResponse>> PrepareImageResposne(ImageUploadResult uploadResult, AppUser user)
        {
            var userImage = new UserImage
            {
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId
            };

            if (user.Profile.Images.Count == 0)
            {
                userImage.IsMain = true;
            }

            user.Profile.Images.Add(userImage);

            _unitOfWork.Repository<UserImage>().Add(userImage);

            if (await _unitOfWork.Complete() != 1)
            {
                _logger.Here().Error($"{ErrorCodes.Operationfailed}: Updating database with new image entry failed");
                return Result<MemberImageResponse>.Fail(ErrorCodes.Operationfailed, ErrorMessages.Operationfailed);
            }

            _logger.Here().Error("Updating database with new image entry success. {@publicId}", userImage.PublicId);
            var result = _mapper.Map<MemberImageResponse>(userImage);
            return Result<MemberImageResponse>.Success(result);
        }
    }
}
