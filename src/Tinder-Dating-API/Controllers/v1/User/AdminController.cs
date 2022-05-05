using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Constants;
using Tinder_Dating_API.Services.User.Admin;

namespace Tinder_Dating_API.Controllers.v1.User
{
    public class AdminController : BaseApiController
    {
        private readonly IAdminUserService _adminUserService;
        public AdminController(ILogger logger, 
            IAdminUserService adminUserService) :
            base(logger)
        {
            _adminUserService = adminUserService;
        }

        [HttpGet("users-with-roles")]
        [Authorize(Policy = ApiAccess.RequireAdminRole)]
        public async Task<IActionResult> GetUserWithRoles()
        {
            Logger.Here().MethoEnterd();

            var result = await _adminUserService.GetUserWithRoles();

            Logger.Here().MethodExited();

            return OkOrFail(result);
        }

        [HttpPut("edit-roles/{username}")]
        [Authorize(Policy = ApiAccess.RequireAdminRole)]
        public async Task<IActionResult> GetPhotosForModeration([FromRoute] string username, [FromQuery] string roles)
        {
            Logger.Here().MethoEnterd();

            var result = await _adminUserService.EditUserRoles(username, roles);

            Logger.Here().MethodExited();
            return OkOrFail(result);
        }
    }
}
