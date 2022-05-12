using Microsoft.Extensions.DependencyInjection;
using Tinder_Dating_API.Infrastructure.Filters;
using Tinder_Dating_API.Services.Identity;
using Tinder_Dating_API.Services.Like;
using Tinder_Dating_API.Services.MemberImage;
using Tinder_Dating_API.Services.User;
using Tinder_Dating_API.Services.User.Admin;

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
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<LogUserActivityFilter>();
            return services;
        }
    }
}
