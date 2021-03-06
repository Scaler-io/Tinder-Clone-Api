using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tinder_Dating_API.DataAccess;
using Tinder_Dating_API.DataAccess.Interfaces;

namespace Tinder_Dating_API.DependencyInjections
{
    public static class ApplicationDataServiceExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlite(config.GetConnectionString("SqliteConnection"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
