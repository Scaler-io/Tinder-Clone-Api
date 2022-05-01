using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Requests;
using Tinder_Dating_API.Models.Responses;
using Tinder_Dating_API.Services.User;

namespace Tinder_Dating_API.Controllers.v1.User
{
    [ApiVersion("1")]
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        public UserController(ILogger logger, IUserService userService)
            : base(logger)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(MemberResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUsers([FromQuery]SpecParams param)
        {
            Logger.Here().MethoEnterd();

            var users = await _userService.GetUsers(param);
            Logger.Here().MethodExited();

            return OkOrFail(users);
        }

        [HttpGet("{id}", Name = "GetUser")]
        [ProducesResponseType(typeof(MemberResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            Logger.Here().MethoEnterd();

            var user = await _userService.GetUser(id);

            Logger.Here().MethodExited();
            return OkOrFail(user);

        }

        [HttpGet("profile/{username}", Name = "GetUserByUserName")]
        [ProducesResponseType(typeof(MemberResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> GetUserByUserName([FromRoute] string username)
        {
            Logger.Here().MethoEnterd();

            var user = await _userService.GetUserByUserName(username);

            Logger.Here().MethodExited();
            return OkOrFail(user);
        }

        [HttpPut("profile/update")]
        [ProducesResponseType(typeof(MemberResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UserDetailsUpdateRequest request)
        {
            Logger.Here().MethoEnterd();
            var result = await _userService.UpdateUserProfile(request);
            Logger.Here().MethodExited();

            return OkOrFail(result);
        }
    }
}
