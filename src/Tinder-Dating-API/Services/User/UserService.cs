using AutoMapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tinder_Dating_API.DataAccess.Interfaces;
using Tinder_Dating_API.DataAccess.Specifications.User;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Core;
using Tinder_Dating_API.Models.Responses;

namespace Tinder_Dating_API.Services.User
{
    public class UserService : IUserService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<UserDetailsResponse>>> GetUsers()
        {
            _logger.Here().MethoEnterd();

            var spec = new GetUserWithProfileInfoSpec();
            var users = await _unitOfWork.Repository<AppUser>().ListAsync(spec);

            if(users == null)
            {
                _logger.Information("No users found in database");
            }

            var result = _mapper.Map<IReadOnlyList<UserDetailsResponse>>(users);
            _logger.Here().MethodExited();

            return Result<IReadOnlyList<UserDetailsResponse>>.Success(result);
        }
        public async Task<Result<UserDetailsResponse>> GetUser(Guid id)
        {
            _logger.Here().MethoEnterd();

            var spec = new GetUserWithProfileInfoSpec(id);
            var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(spec);

            if (user == null)
            {
                _logger.Information("No user was found in database with {@Id}", id);
            }

            var result = _mapper.Map<UserDetailsResponse>(user);
            _logger.Here().MethodExited();

            return Result<UserDetailsResponse>.Success(result);
        }

    }
}
