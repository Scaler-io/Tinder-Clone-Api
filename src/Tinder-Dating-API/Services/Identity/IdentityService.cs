using Serilog;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tinder_Dating_API.DataAccess.Interfaces;
using Tinder_Dating_API.DataAccess.Specifications.Identity;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Constants;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Requests;
using Tinder_Dating_API.Models.Responses;

namespace Tinder_Dating_API.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public IdentityService(ILogger logger, IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<Result<AuthSuccessResponse>> Login(LoginRequest request)
        {
            _logger.Here().MethoEnterd();

            var specification = new FindUserByUserNameSpec(request.Username);
            var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(specification);

            if (user == null)
            {
                _logger.Here().Information($"{ErrorCodes.Operationfailed}: Login failed due to wrong credential.");
                return Result<AuthSuccessResponse>.Fail(ErrorCodes.Unauthorized, "Invalid credentials given.");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

            for(int i=0; i<computedHash.Length; i++)
            {
                if (user.PasswordHash[i] != computedHash[i])
                {
                    _logger.Here().Information($"{ErrorCodes.Operationfailed}: Login failed due to wrong credential.");
                    return Result<AuthSuccessResponse>.Fail(ErrorCodes.Unauthorized, "Invalid credentials given.");
                }
            }

            _logger.Information("Login to system successfull.");
            _logger.Here().MethodExited();

            var authSuccess = new AuthSuccessResponse
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

            return Result<AuthSuccessResponse>.Success(authSuccess);
        }

        public async Task<Result<AuthSuccessResponse>> Register(RegisterUserRequest request)
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

            var token = _tokenService.CreateToken(user);
            var authSuccess = new AuthSuccessResponse
            {
                Username = user.UserName,
                Token = token
            };

            _logger.Information("New user has been created successfully. {@user}", user);
            _logger.Here().MethodExited();


            return Result<AuthSuccessResponse>.Success(authSuccess);
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
