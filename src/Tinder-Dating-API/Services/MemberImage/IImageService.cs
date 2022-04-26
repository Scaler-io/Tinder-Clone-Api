using CloudinaryDotNet.Actions;
using System.Threading.Tasks;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Requests.Photo;
using Tinder_Dating_API.Models.Responses;

namespace Tinder_Dating_API.Services.MemberImage
{
    public interface IImageService
    {
        Task<Result<MemberImageResponse>> AddImageAsync(UploadPhotoRequest request);
        Task<Result<DeletionResult>> DeleteImageAsync(DeletePhotoRequest request);
        Task<Result<bool>> UpdatePhotoAsMain(UpdatePhotoRequest request);
    }
}
