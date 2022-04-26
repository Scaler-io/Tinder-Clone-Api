using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Requests.Photo;
using Tinder_Dating_API.Services.MemberImage;
using Tinder_Dating_API.Models.Responses;
using Tinder_Dating_API.Models.Core;
using System.Net;

namespace Tinder_Dating_API.Controllers.v1.Photo
{
    [Authorize]
    [ApiVersion("1")]
    public class PhotoController : BaseApiController
    {
        private readonly IImageService _imageService;
        public PhotoController(ILogger logger, 
            IImageService imageService)
            : base(logger)
        {
            _imageService = imageService;
        }

        [HttpPost("upload")]
        [ProducesResponseType(typeof(MemberImageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UploadPhoto([FromForm]UploadPhotoRequest request)
        {
            Logger.MethoEnterd();

            var username = User.GetAuthUserName(); 
            var result = await _imageService.AddImageAsync(request);

            Logger.MethodExited();

            return CreatedWithRoute(result,
                "GetUserByUserName", 
                new { username = username }
            );
        }
    
        [HttpPut("set-main-photo/{PhotoId}")]
        [ProducesResponseType(typeof(MemberImageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SetAsMainPhoto([FromRoute] UpdatePhotoRequest request)
        {
            Logger.Here().MethoEnterd();

            var result = await _imageService.UpdatePhotoAsMain(request);

            Logger.Here().MethodExited();

            return OkOrFail(result);
        }

        [HttpDelete("delete-photo/{PhotoId}")]
        [ProducesResponseType(typeof(MemberImageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeletePhoto([FromRoute] DeletePhotoRequest request)
        {
            Logger.Here().MethoEnterd();

            var result = await _imageService.DeleteImageAsync(request);

            Logger.Here().MethodExited();

            return OkOrFail(result);
        } 
    }
}
