using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Threading.Tasks;
using Tinder_Dating_API.DataAccess;
using Tinder_Dating_API.Extensions;
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
        public async Task<IActionResult> GetUsers()
        {
            Logger.Here().MethoEnterd();

            var users = await _userService.GetUsers();
            Logger.Here().MethodExited();

            return OkOrFail(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            Logger.Here().MethoEnterd();

            var user = await _userService.GetUser(id);

            Logger.Here().MethodExited();
            return OkOrFail(user);
        }
    }
}
