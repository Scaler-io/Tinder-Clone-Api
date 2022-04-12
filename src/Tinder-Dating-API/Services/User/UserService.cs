using AutoMapper;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Tinder_Dating_API.DataAccess.Interfaces;
using Tinder_Dating_API.DataAccess.Specifications.User;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Constants;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Requests;
using Tinder_Dating_API.Models.Responses;

namespace Tinder_Dating_API.Services.User
{
    public class UserService : IUserService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            ILogger logger, 
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<IReadOnlyList<MemberResponse>>> GetUsers()
        {
            _logger.Here().MethoEnterd();

            var spec = new GetUserWithProfileInfoSpec();
            var users = await _unitOfWork.Repository<AppUser>().ListAsync(spec);

            if(users == null)
            {
                _logger.Information("No users found in database");
            }

            var result = _mapper.Map<IReadOnlyList<MemberResponse>>(users);
            _logger.Here().MethodExited();

            return Result<IReadOnlyList<MemberResponse>>.Success(result);
        }
        public async Task<Result<MemberResponse>> GetUser(Guid id)
        {
            _logger.Here().MethoEnterd();

            var spec = new GetUserWithProfileInfoSpec(id);
            var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(spec);

            if (user == null)
            {
                _logger.Information("No user was found in database with {@Id}", id);
            }

            var result = _mapper.Map<MemberResponse>(user);
            _logger.Here().MethodExited();

            return Result<MemberResponse>.Success(result);
        }
        public async Task<Result<MemberResponse>> GetUserByUserName(string username)
        {
            _logger.Here().MethoEnterd();

            var spec = new FindUserByUserNameSpec(username);
            var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(spec);

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
            var repository = _unitOfWork.Repository<AppUser>();
            
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            var spec = new FindUserByUserNameSpec(username);
            var user = await repository.GetEntityWithSpec(spec);

            _mapper.Map(request, user.Profile);
            //user.Profile.Address = request.Address;
            
            
            repository.Update(user);

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
    }
}
