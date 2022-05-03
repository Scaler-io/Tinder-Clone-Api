using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Extensions;

namespace Tinder_Dating_API.DataAccess
{
    public class AppContextSeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, ILogger logger)
        {
            if(!await userManager.Users.AnyAsync())
            {

                var userData = File.ReadAllText("./DataAccess/Seeders/Users.json");
                var users = JsonConvert.DeserializeObject<List<AppUser>>(userData);

                foreach(var user in users)
                {
                    user.UserName = user.UserName.ToLower();
                    await userManager.CreateAsync(user, "P@ssw0rd");
                }
            }

            logger.Here().Information("Seed database associated with context {@DbContextName}", typeof(ApplicationDbContext).Name);
        }
    }
}
