using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;
using Tinder_Dating_API.DataAccess;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Extensions;

namespace Tinder_Dating_API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                await MigrateDbContextAsync(host);

                SeedApplicationDbContext(host);

                await host.RunAsync();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task MigrateDbContextAsync(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger>();

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    await context.Database.MigrateAsync();
                }
                catch (Exception e)
                {
                    logger.Error(e, "An error occured while migrating database associated with {DbContext}"
                        , typeof(ApplicationDbContext).Name);
                }
            }
        }

        private static void SeedApplicationDbContext(IHost host)
        {
            host.SeedDatabase<ApplicationDbContext>((context, services) =>
            {
                var logger = services.GetRequiredService<ILogger>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                AppContextSeed.SeedAsync(userManager, roleManager, logger).Wait();
            });
        }
    }
}
