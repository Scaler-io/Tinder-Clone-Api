using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;
using Tinder_Dating_API.DataAccess.Interfaces;
using Tinder_Dating_API.DataAccess.Specifications.Like;
using Tinder_Dating_API.DataAccess.Specifications.User;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Constants;
using Tinder_Dating_API.Models.Core;

namespace Tinder_Dating_API.Services.Like
{
    public class LikeService : ILikeService
    {
        private readonly ILogger _logger;
        private readonly IBaseRepository<UserLike> _userLikeRepository;
        private readonly IBaseRepository<AppUser> _userRespository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUnitOfWork _unitOfWork;

        public LikeService(ILogger logger,
            IHttpContextAccessor httpContext, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRespository = _unitOfWork.Repository<AppUser>();
            _userLikeRepository = _unitOfWork.Repository<UserLike>();
            _httpContext = httpContext;   
        }

        public async Task<Result<bool>> AddLike(string username)
        {
            _logger.Here().MethoEnterd();

            var currentUser = _httpContext.HttpContext.User;

            var likedUser = await GetLikedUser(username);
            var sourceUser = await GetSourceUser(currentUser.GetAuthUserId());

            if(likedUser == null)
            {
                _logger.Here().Information($"{ErrorCodes.NotFound}: liked user {username} was not found.");
                return Result<bool>.Fail(ErrorCodes.NotFound, $"Liked user {username} was not found");
            }

            if(sourceUser.UserName.ToLower() == username.ToLower())
            {
                _logger.Here().Information($"{ErrorCodes.BadRequest}: auth user cannot like him/herself.");
                return Result<bool>.Fail(ErrorCodes.BadRequest, "You cannot like yourself");
            }

            var userLike = await GetUserLike(sourceUser.Id, likedUser.Id);
            if(userLike != null)
            {
                _logger.Here().Information($"The user {username} is already liked.");
                return Result<bool>.Fail(ErrorCodes.BadRequest, $"The user {username} is already liked.");
            }

            userLike = new UserLike
            {
                SourceUserId = sourceUser.Id,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);
            if(await _unitOfWork.Complete() > 0)
            {
                _logger.Here().Information($"user {username} is liked by {sourceUser.UserName}");
                return Result<bool>.Success(true);
            }

            _logger.Here().Information($"{ErrorCodes.Operationfailed}: Failed to like user {username}");
            _logger.Here().MethodExited();
            return Result<bool>.Fail(ErrorCodes.BadRequest,"Failed to like user {username}");
        }

        private async Task<AppUser> GetLikedUser(string username)
        {
            var spec = new FindUserByUserNameSpec(username);
            return await _userRespository.GetEntityWithSpec(spec);
        }

        private async Task<AppUser> GetSourceUser(Guid sourceUserId)
        {
            var spec = new GetUserWithLikesSpec(sourceUserId);
            return await _userRespository.GetEntityWithSpec(spec);
        }

        private async Task<UserLike> GetUserLike(Guid sourceId, Guid likedUserId)
        {
            var spec = new UserLikeSpec(sourceId, likedUserId);
            return await _userLikeRepository.GetEntityWithSpec(spec);
        }
    }
}
