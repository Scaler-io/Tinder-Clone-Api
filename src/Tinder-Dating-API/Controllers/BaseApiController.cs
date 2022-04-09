using Microsoft.AspNetCore.Mvc;
using Serilog;
using Tinder_Dating_API.Models.Constants;
using Tinder_Dating_API.Models.Core;

namespace Tinder_Dating_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        public ILogger Logger { get; set; }
        public BaseApiController(ILogger logger)
        {
            Logger = logger;
        }

        public IActionResult OkOrFail<T>(Result<T> result)
        {
            if (result.IsSuccess && result.Value == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);

            switch (result.ErrorCode)
            {
                case ErrorCodes.NotFound:
                    return NotFound(new ApiResponse(ErrorCodes.NotFound));
                case ErrorCodes.Unauthorized:
                    return Unauthorized(new ApiResponse(ErrorCodes.Unauthorized));
                case ErrorCodes.Operationfailed:
                    return BadRequest(new ApiResponse(ErrorCodes.Operationfailed, ErrorMessages.Operationfailed));
                default:
                    return BadRequest(new ApiResponse(ErrorCodes.BadRequest, result.ErrorMessage));
            }
        }
    }
}
