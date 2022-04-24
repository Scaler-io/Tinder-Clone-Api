using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Requests.Photo;
using Tinder_Dating_API.Services.MemberImage;
using Microsoft.AspNetCore.Http;

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
        public async Task<IActionResult> UploadPhoto([FromForm]UploadPhotoRequest request)
        {
            Logger.MethoEnterd();

            var result = await _imageService.AddImageAsync(request);

            Logger.MethodExited();
            return OkOrFail(result);
            //return Ok(request.File);
        }
    }
}
