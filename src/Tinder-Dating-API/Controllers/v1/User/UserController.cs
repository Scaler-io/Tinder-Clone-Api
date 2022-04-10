using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Responses;
using Tinder_Dating_API.Services.User;

namespace Tinder_Dating_API.Controllers.v1.User
{
    [ApiVersion("1")]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        public UserController(ILogger logger, IUserService userService)
            : base(logger)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDetailsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {
            Logger.Here().MethoEnterd();

            var users = await _userService.GetUsers();
            Logger.Here().MethodExited();

            return OkOrFail(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(UserDetailsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            Logger.Here().MethoEnterd();

            var user = await _userService.GetUser(id);

            Logger.Here().MethodExited();
            return OkOrFail(user);

        }
    }
}
