using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Models.Core;

namespace Tinder_Dating_API.Services.User
{
    public interface IUserService
    {
        Task<Result<IReadOnlyList<AppUser>>> GetUsers();
        Task<Result<AppUser>> GetUser(Guid id);
    }
}
