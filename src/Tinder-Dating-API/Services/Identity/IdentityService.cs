using Serilog;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tinder_Dating_API.DataAccess;
using Tinder_Dating_API.DataAccess.Interfaces;
using Tinder_Dating_API.DataAccess.Specifications.Identity;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Requests;

namespace Tinder_Dating_API.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;

        public IdentityService(ILogger logger, IUnitOfWork unitOfWork, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public async Task<Result<AppUser>> Register(RegisterUserRequest request)
        {
            _logger.Here().MethoEnterd();
            
            using var hmac = new HMACSHA512();
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = hmac.Key
            };

            _unitOfWork.Repository<AppUser>().Add(user);
            await _unitOfWork.Complete();

            _logger.Information("New user has been created successfully. {@user}", user);

            _logger.Here().MethodExited();

            return Result<AppUser>.Success(user);
        }

        public async Task<bool> IsUserNameExists(string username)
        {
            _logger.Here().MethoEnterd();

            var specification = new FindUserByUserNameSpec(username);
            var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(specification);

            if (user == null) return false;

            _logger.Here().MethodExited();
            return true;
        }
    }
}
