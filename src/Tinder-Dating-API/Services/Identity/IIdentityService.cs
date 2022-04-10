using System.Threading.Tasks;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Requests;
using Tinder_Dating_API.Models.Responses;

namespace Tinder_Dating_API.Services.Identity
{
    public interface IIdentityService
    {
        Task<Result<AuthSuccessResponse>> Register(RegisterUserRequest request);
        Task<Result<AuthSuccessResponse>> Login(LoginRequest request);
        Task<bool> IsUserNameExists(string username);
    }
}
