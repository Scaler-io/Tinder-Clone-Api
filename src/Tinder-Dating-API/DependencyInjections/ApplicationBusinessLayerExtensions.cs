using Microsoft.Extensions.DependencyInjection;
using Tinder_Dating_API.Services.Identity;
using Tinder_Dating_API.Services.MemberImage;
using Tinder_Dating_API.Services.User;

namespace Tinder_Dating_API.DependencyInjections
{
    public static class ApplicationBusinessLayerExtensions
    {
        public static IServiceCollection AddBusinessLayerServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IImageService, ImageService>();
            return services;
        }
    }
}
