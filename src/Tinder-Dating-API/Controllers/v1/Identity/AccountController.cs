using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Constants;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Requests;
using Tinder_Dating_API.Services.Identity;

namespace Tinder_Dating_API.Controllers.v1.Identity
{
    [ApiVersion("1")]
    public class AccountController : BaseApiController
    {
        private readonly IIdentityService _identityService;

        public AccountController(ILogger logger, IIdentityService identityService)
            : base(logger)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewUser([FromBody] RegisterUserRequest request)
        {
            Logger.Here().MethoEnterd();

            if (await _identityService.IsUserNameExists(request.UserName))
            {
                Logger.Here().Information("{@ErrorCode}: Registration failed. username already taken. {@username}",
                    ErrorCodes.Operationfailed, request.UserName);
                return CreateDuplicateUsernameResposne(request.UserName);
            }

            var result = await _identityService.Register(request); 

            Logger.Here().MethodExited();
            return OkOrFail(result);
        }
        
        [HttpGet("IsUsernameExists")]   
        public async Task<ActionResult<bool>> CheckIfUsernameExists([FromQuery]string username)
        {
            Logger.Here().MethoEnterd();
            
            var result = await _identityService.IsUserNameExists(username);

            Logger.Here().MethodExited();

            return result;
        } 
        

        private IActionResult CreateDuplicateUsernameResposne(string username)
        {
            return UnprocessableEntity(new ApiValidationResponse
            {
                Code = ErrorCodes.Operationfailed,
                Errors = new List<FieldLevelError> { 
                    new FieldLevelError
                    {
                        Code = ErrorCodes.UnprocessableEntity,
                        Field = "username",
                        Message = $"Username '{username}' is already taken."
                    }
                }
            });
        }
    }
}
