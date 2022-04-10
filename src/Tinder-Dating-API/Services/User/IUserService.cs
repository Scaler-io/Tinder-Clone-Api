using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Responses;

namespace Tinder_Dating_API.Services.User
{
    public interface IUserService
    {
        Task<Result<IReadOnlyList<UserDetailsResponse>>> GetUsers();
        Task<Result<UserDetailsResponse>> GetUser(Guid id);
    }
}
