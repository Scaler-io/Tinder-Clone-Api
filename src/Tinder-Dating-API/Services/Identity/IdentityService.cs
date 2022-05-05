using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly SignInManager<AppUser> _signinManager;
        private readonly UserManager<AppUser> _userManager;

        public IdentityService(ILogger logger,
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor, 
            IMapper mapper, 
            SignInManager<AppUser> signinManager, 
            UserManager<AppUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = _unitOfWork.Repository<AppUser>();
            _mapper = mapper;
            _signinManager = signinManager;
            _userManager = userManager;
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

            var result = await _signinManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded) {
                _logger.Here().Information($"{ErrorCodes.Operationfailed}: Login attempt failed for username {request.Username}");
                return Result<AuthSuccessResponse>.Fail(ErrorCodes.Unauthorized);
            }   

            var authSuccess = await GenerateAuthSuccessResponse(user);

            _logger.Information("Login to system successfull.");
            _logger.Here().MethodExited();

            return Result<AuthSuccessResponse>.Success(authSuccess);
        }

        public async Task<Result<AuthSuccessResponse>> Register(RegisterUserRequest request)
        {
            _logger.Here().MethoEnterd();

            var user = PopulateNewUser(request);

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                _logger.Here().Information($"{ErrorCodes.Operationfailed}: User registration failed. {result.Errors}");
                return Result<AuthSuccessResponse>.Fail(ErrorCodes.BadRequest, "User registration failed.");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, ApplicationRoles.Member);
            if (!roleResult.Succeeded)
            {
                _logger.Here().Information($"{ErrorCodes.Operationfailed}: Role assignement to user failed. {result.Errors}");
                return Result<AuthSuccessResponse>.Fail(ErrorCodes.BadRequest, "User registration failed.");
            }

            var authSuccess = await GenerateAuthSuccessResponse(user);

            _logger.Information("New user has been created successfully. {@user}", user);
            _logger.Here().MethodExited();

            return Result<AuthSuccessResponse>.Success(authSuccess);
        }

        public async Task<bool> IsUserNameExists(string username)
        {
            _logger.Here().MethoEnterd();

            var userExists = await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());

            if (!userExists) return false;

            _logger.Here().MethodExited();
            return true;
        }
        public async Task<AppUser> GetCurrentAuthUser()
        {
            var userId    =      _httpContextAccessor
                                    ?.HttpContext
                                    ?.User
                                    ?.GetAuthUserId();


            var spec = new GetUserWithProfileInfoSpec(userId.Value);
            return await _userRepository.GetEntityWithSpec(spec);
        }
    
        private async Task<AuthSuccessResponse> GenerateAuthSuccessResponse(AppUser user)
        {
            return new AuthSuccessResponse
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                Gender = user.Profile.Gender,
                PhotoUrl = user.Profile?.Images?.FirstOrDefault(m => m.IsMain)?.Url,
                KnownAs = user.Profile.KnownAs
            };
        }
        private AppUser PopulateNewUser(RegisterUserRequest request)
        {
            
            var userProfile = _mapper.Map<UserProfile>(request.Profile);
            userProfile.Created = DateTime.Now;
            userProfile.LastActive = DateTime.Now;

            return new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName.ToLower(),
                Profile = userProfile
            };
        } 

    }
}
