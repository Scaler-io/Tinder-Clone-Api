using AutoMapper;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tinder_Dating_API.DataAccess.Interfaces;
using Tinder_Dating_API.DataAccess.Specifications.User;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseRepository<AppUser> _userRepository;
        private readonly IMapper _mapper;

        public IdentityService(ILogger logger,
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = _unitOfWork.Repository<AppUser>();
            _mapper = mapper;
        }

        public async Task<Result<AuthSuccessResponse>> Login(LoginRequest request)
        {
            _logger.Here().MethoEnterd();

            var specification = new FindUserByUserNameSpec(request.Username);
            var user = await _userRepository.GetEntityWithSpec(specification);

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

            user.Profile.LastActive = DateTime.Now;
            _userRepository.Update(user);
            await _unitOfWork.Complete();

            _logger.Information("Login to system successfull.");
            _logger.Here().MethodExited();

            var authSuccess = GenerateAuthSuccessResponse(user);

            return Result<AuthSuccessResponse>.Success(authSuccess);
        }

        public async Task<Result<AuthSuccessResponse>> Register(RegisterUserRequest request)
        {
            _logger.Here().MethoEnterd();

            var user = PopulateNewUser(request);

            _userRepository.Add(user);
            await _unitOfWork.Complete();

            var authSuccess = GenerateAuthSuccessResponse(user);

            _logger.Information("New user has been created successfully. {@user}", user);
            _logger.Here().MethodExited();

            return Result<AuthSuccessResponse>.Success(authSuccess);
        }

        public async Task<bool> IsUserNameExists(string username)
        {
            _logger.Here().MethoEnterd();

            var specification = new FindUserByUserNameSpec(username);
            var user = await _userRepository.GetEntityWithSpec(specification);

            if (user == null) return false;

            _logger.Here().MethodExited();
            return true;
        }
        public async Task<AppUser> GetCurrentAuthUser()
        {
            var username    =      _httpContextAccessor
                                    ?.HttpContext
                                    ?.User
                                    ?.GetAuthUserName();


            var spec = new FindUserByUserNameSpec(username);
            return await _userRepository.GetEntityWithSpec(spec);
        }
    
        private AuthSuccessResponse GenerateAuthSuccessResponse(AppUser user)
        {
            return new AuthSuccessResponse
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                Gender = user.Profile.Gender,
                PhotoUrl = user.Profile?.Images?.FirstOrDefault(m => m.IsMain)?.Url,
                KnownAs = user.Profile.KnownAs
            };
        }
        private AppUser PopulateNewUser(RegisterUserRequest request)
        {
            using var hmac = new HMACSHA512();
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

            var userProfile = _mapper.Map<UserProfile>(request.Profile);
            userProfile.Created = DateTime.Now;
            userProfile.LastActive = DateTime.Now;

            return new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = hmac.Key,
                Profile = userProfile
            };
        } 
    }
}
