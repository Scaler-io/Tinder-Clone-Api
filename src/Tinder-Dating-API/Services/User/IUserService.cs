using System;
using System.Threading.Tasks;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Requests;
using Tinder_Dating_API.Models.Responses;

namespace Tinder_Dating_API.Services.User
{
    public interface IUserService
    {
        Task<Result<Pagination<MemberResponse>>> GetUsers(SpecParams param);
        Task<Result<MemberResponse>> GetUser(Guid id);
        Task<Result<MemberResponse>> GetUserByUserName(string username);
        Task<Result<MemberResponse>> UpdateUserProfile(UserDetailsUpdateRequest request);
    }
}
