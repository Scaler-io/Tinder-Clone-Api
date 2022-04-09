using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tinder_Dating_API.DataAccess.Interfaces;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Core;

namespace Tinder_Dating_API.Services.User
{
    public class UserService : IUserService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(ILogger logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IReadOnlyList<AppUser>>> GetUsers()
        {
            _logger.Here().MethoEnterd();

            var users = await _unitOfWork.Repository<AppUser>().ListAllAsync();

            if(users == null)
            {
                _logger.Information("No users found in database");
            }

            _logger.Here().MethodExited();

            return Result<IReadOnlyList<AppUser>>.Success(users);
        }
        public async Task<Result<AppUser>> GetUser(Guid id)
        {
            _logger.Here().MethoEnterd();

            var user = await _unitOfWork.Repository<AppUser>().GetByIdAsync(id);

            if (user == null)
            {
                _logger.Information("No user was found in database with {@Id}", id);
            }

            _logger.Here().MethodExited();

            return Result<AppUser>.Success(user);
        }

    }
}
