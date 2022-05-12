using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Services.Like;

namespace Tinder_Dating_API.Controllers.v1.Likes
{
    [Authorize]
    public class LikeController: BaseApiController
    {
        private readonly ILikeService _likeService;

        public LikeController(ILogger logger, 
            ILikeService likeService)
            : base(logger)
        {
            _likeService = likeService;
        }

        [HttpPost("{username}")]
        public async Task<IActionResult> AddLike([FromRoute] string username)
        {
            Logger.Here().MethoEnterd();

            var result = await _likeService.AddLike(username);

            Logger.Here().MethodExited();
            return OkOrFail(result);
        }
    }
}
