using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Threading.Tasks;
using Tinder_Dating_API.DataAccess.Seeders;
using Tinder_Dating_API.Extensions;

namespace Tinder_Dating_API.DataAccess
{
    public class AppContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
        {
            if(!await context.Users.AnyAsync())
            {
                await context.Users.AddRangeAsync(UserSeeder.Users());
                await context.SaveChangesAsync();
            }

            logger.Here().Information("Seed database associated with context {@DbContextName}", typeof(ApplicationDbContext).Name);
        }
    }
}
