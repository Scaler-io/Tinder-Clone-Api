using Microsoft.Extensions.DependencyInjection;
using Tinder_Dating_API.Services.User;

namespace Tinder_Dating_API.DependencyInjections
{
    public static class ApplicationBusinessLayerExtensions
    {
        public static IServiceCollection AddBusinessLayerServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            return services;
        }
    }
}
