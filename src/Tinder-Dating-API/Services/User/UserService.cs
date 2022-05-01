using AutoMapper;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Tinder_Dating_API.DataAccess.Interfaces;
using Tinder_Dating_API.DataAccess.Specifications.User;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Constants;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Requests;
using Tinder_Dating_API.Models.Responses;
using Tinder_Dating_API.Services.Identity;

namespace Tinder_Dating_API.Services.User
{
    public class UserService : IUserService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseRepository<AppUser> _userRepository;
        private readonly IIdentityService _identityService;

        public UserService(
            ILogger logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IIdentityService identityService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = _unitOfWork.Repository<AppUser>();
            _identityService = identityService;
        }

        public async Task<Result<Pagination<MemberResponse>>> GetUsers(SpecParams param)
        {
            _logger.Here().MethoEnterd();

            var filteredParam = await ApplyFilter(param);

            var spec = new GetUserWithProfileInfoSpec(filteredParam);
            var countSpec = new UserWithFiltersCountSpec(filteredParam);

            var users = await _userRepository.ListAsync(spec);
            var totalItems = await _userRepository.CountAsync(countSpec);
   
            if(users == null)
            {
                _logger.Here().Information("No users found in database");
                return null;
            }

            SetupPaginationHeader(param, totalItems);

            var result = _mapper.Map<IReadOnlyList<MemberResponse>>(users);

            _logger.Here().MethodExited();

            return Result<Pagination<MemberResponse>>.Success(new Pagination<MemberResponse>(
                param.PageIndex, param.PageSize, totalItems, result    
            ));
        }
        public async Task<Result<MemberResponse>> GetUser(Guid id)
        {
            _logger.Here().MethoEnterd();

            var spec = new GetUserWithProfileInfoSpec(id);
            var user = await _userRepository.GetEntityWithSpec(spec);

            if (user == null)
            {
                _logger.Here().Information("No user was found in database with {@Id}", id);
            }

            var result = _mapper.Map<MemberResponse>(user);
            _logger.Here().MethodExited();

            return Result<MemberResponse>.Success(result);
        }
        public async Task<Result<MemberResponse>> GetUserByUserName(string username)
        {
            _logger.Here().MethoEnterd();

            var spec = new FindUserByUserNameSpec(username);
            var user = await _userRepository.GetEntityWithSpec(spec);

            if (user == null)
            {
                _logger.Here().Information("No user was found in database with {@UserName}", username);
            }

            var result = _mapper.Map<MemberResponse>(user);

            _logger.Here().MethodExited();
            return Result<MemberResponse>.Success(result);
        }
        public async Task<Result<MemberResponse>> UpdateUserProfile(UserDetailsUpdateRequest request)
        {
            _logger.Here().MethoEnterd();

            var username = _httpContextAccessor.HttpContext.User.GetAuthUserName();

            var spec = new FindUserByUserNameSpec(username);
            var user = await _userRepository.GetEntityWithSpec(spec);

            _mapper.Map(request, user.Profile);
            //user.Profile.Address = request.Address;
            
            _userRepository.Update(user);

            if (await _unitOfWork.Complete() < 1)
            {
                _logger.Here().Error($"{ErrorCodes.Operationfailed}: User profile update failed.");
                return Result<MemberResponse>.Fail(ErrorCodes.Operationfailed);
            }

            var userDetails = await GetUserByUserName(username);

            _logger.Here().Information("User profile updated successfully. {@user}", userDetails.Value);
            _logger.Here().MethodExited();

            return userDetails;
        }
        private void SetupPaginationHeader(SpecParams param, int totalItems)
        {
            var response = _httpContextAccessor.HttpContext.Response;
            var totalPages = (int)Math.Ceiling((double)totalItems / param.PageSize);
            response.AddPaginationResponseHeader(param.PageIndex, param.PageSize, totalItems, totalPages);
        }
    
        private async Task<SpecParams> ApplyFilter(SpecParams param)
        {
            var currentUser = await _identityService.GetCurrentAuthUser();

            param.CurrentUserName = currentUser.UserName;

            param.MinDob = DateTime.Now.AddYears(-param.MaxAge);
            param.MaxDob = DateTime.Now.AddYears(-param.MinAge);

            //var obj = new
            //{
            //    curAge = currentUser.Profile.GetAge(),
            //    cur = currentUser.Profile.DateOfBirth.Year,
            //    parMin = param.MinDob.Year,
            //    parMax = param.MaxDob.Year
            //};


            //await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync
            //        (obj);

            // applies gender filter
            var gender = currentUser.Profile.Gender;

            if (string.IsNullOrEmpty(param.Gender))
                param.Gender = gender == "male" ? "female" : "male";

            return param;
        }
    }
}
