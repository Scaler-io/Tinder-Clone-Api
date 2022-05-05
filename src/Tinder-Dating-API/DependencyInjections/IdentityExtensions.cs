using Microsoft.Extensions.DependencyInjection;
using Tinder_Dating_API.Models.Constants;

namespace Tinder_Dating_API.DependencyInjections
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddApplicationAuthorizationServices(
                this IServiceCollection services
            )
        {
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(ApiAccess.RequireAdminRole, policy => policy.RequireRole(ApplicationRoles.Administrator));
                opt.AddPolicy(ApiAccess.ModeratePhotoRole, policy => policy.RequireRole(ApplicationRoles.Administrator, ApplicationRoles.Moderator));
            });
            return services;
        }
    }
}
