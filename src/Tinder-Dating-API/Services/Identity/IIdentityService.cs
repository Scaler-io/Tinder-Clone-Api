using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Requests;

namespace Tinder_Dating_API.Services.Identity
{
    public interface IIdentityService
    {
        Task<Result<AppUser>> Register(RegisterUserRequest request);
        Task<bool> IsUserNameExists(string username);
    }
}
