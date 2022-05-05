using System.Collections.Generic;
using System.Threading.Tasks;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Requests;
using Tinder_Dating_API.Models.Responses;

namespace Tinder_Dating_API.Services.User.Admin
{
    public interface IAdminUserService
    {
        Task<Result<List<UserAuthDetailsResponse>>> GetUserWithRoles();
        Task<Result<List<string>>> EditUserRoles(string username, string roles);
    }
}
