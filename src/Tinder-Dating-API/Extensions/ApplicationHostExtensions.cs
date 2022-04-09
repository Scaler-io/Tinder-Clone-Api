using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Tinder_Dating_API.Extensions
{
    public static class ApplicationHostExtensions
    {
        public static IHost SeedDatabase<TContext>(
            this IHost host,
            Action<TContext, IServiceProvider> seeder, 
            int? retryCount = 0
            ) where TContext : DbContext
        {
            var retryForAvailability = retryCount.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger>();
                var context = services.GetRequiredService<TContext>();

                try
                {
                    logger.Information("Migrating database associated with context {DbContextName}", typeof(TContext).Name);
                    InvokeSeeder<TContext>(seeder, context, services);
                    logger.Information("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }catch(Exception e)
                {
                    logger.Error(e, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        SeedDatabase<TContext>(host, seeder, retryForAvailability);
                    }
                }
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(
            Action<TContext, IServiceProvider> seeder,
            TContext context, IServiceProvider services
            ) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
