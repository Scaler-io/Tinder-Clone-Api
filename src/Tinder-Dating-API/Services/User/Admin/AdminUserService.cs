using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Constants;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Responses;
using Tinder_Dating_API.Services.Identity;

namespace Tinder_Dating_API.Services.User.Admin
{
    public class AdminUserService : IAdminUserService
    {
        private readonly ILogger _logger;
        private readonly IIdentityService _identityService;
        private readonly UserManager<AppUser> _userManager;

        public AdminUserService(
            ILogger logger,
            UserManager<AppUser> userManager, 
            IIdentityService identityService)
        {
            _logger = logger;
            _userManager = userManager;
            _identityService = identityService;
        }

        public async Task<Result<List<UserAuthDetailsResponse>>> GetUserWithRoles()
        {
            _logger.Here().MethoEnterd();

            var users = await _userManager.Users
                              .Include(u => u.UserRoles)
                              .ThenInclude(ur => ur.Role)
                              .OrderBy(u => u.UserName)
                              .Select(s => new UserAuthDetailsResponse
                              {
                                  Id = s.Id,
                                  UserName = s.UserName,
                                  Roles = s.UserRoles.Select(r => r.Role.Name).ToList()
                              }).ToListAsync();

            if(users == null)
            {
                _logger.Here().Information("No users found in database");
                return null;
            }


            _logger.Here().MethodExited();
            return Result<List<UserAuthDetailsResponse>>.Success(users);
        }

        public async Task<Result<List<string>>> EditUserRoles(string username, string roles)
        {
            _logger.Here().MethoEnterd();

            var currentUser = await _identityService.GetCurrentAuthUser();

            if(string.Compare(currentUser.UserName, username, true) == 0 )
            {
                return Result<List<string>>.Fail(ErrorCodes.BadRequest, "Own roles cannot be modified");
            }

            if (string.IsNullOrEmpty(roles))
            {
                _logger.Here().Information("No roles provided.");
                return Result<List<string>>.Fail(ErrorCodes.BadRequest, "No roles provided to edit.");
            }

            var selectedRoles = roles.Split(',').ToArray();

            var user = await _userManager.FindByNameAsync(username);
            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if (!result.Succeeded)
            {
                _logger.Here().Information("Role assignment failed.");
                return Result<List<string>>.Fail(ErrorCodes.BadRequest, "Role assignment failed");
            }

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!result.Succeeded)
            {
                _logger.Here().Information("Failed to remove roles.");
                return Result<List<string>>.Fail(ErrorCodes.BadRequest, "Failed to remove roles.");
            }

            var rolesAssigned = await _userManager.GetRolesAsync(user);

            _logger.Here().Information("Roles assigned to user {@userName} {@Roles}", user.UserName, rolesAssigned);
            _logger.Here().MethodExited();

            return Result<List<string>>.Success(rolesAssigned.ToList());
        }

    }
}
