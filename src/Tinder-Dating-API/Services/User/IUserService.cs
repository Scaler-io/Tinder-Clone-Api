using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Requests;
using Tinder_Dating_API.Models.Responses;

namespace Tinder_Dating_API.Services.User
{
    public interface IUserService
    {
        Task<Result<IReadOnlyList<MemberResponse>>> GetUsers();
        Task<Result<MemberResponse>> GetUser(Guid id);
        Task<Result<MemberResponse>> GetUserByUserName(string username);
        Task<Result<MemberResponse>> UpdateUserProfile(UserDetailsUpdateRequest request);
    }
}
