using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Extensions;
using Tinder_Dating_API.Models.Constants;

namespace Tinder_Dating_API.DataAccess
{
    public class AppContextSeed
    {
        public static async Task SeedAsync(
            UserManager<AppUser> userManager, 
            RoleManager<AppRole> roleManager,   
            ILogger logger)
        {
            if(!await userManager.Users.AnyAsync())
            {

                var userData = File.ReadAllText("./DataAccess/Seeders/Users.json");
                var users = JsonConvert.DeserializeObject<List<AppUser>>(userData);

                if (users == null) return;

                // seed roles to db
                await SeedRoles(roleManager);

                foreach (var user in users)
                {
                    user.UserName = user.UserName.ToLower();
                    await userManager.CreateAsync(user, "P@ssw0rd");
                    await userManager.AddToRoleAsync(user, ApplicationRoles.Member);
                }

                // seeds an admin user
                var adminUser = new AppUser { UserName = "Admin" };
                await userManager.CreateAsync(adminUser, "P@ssw0rd");
                await userManager.AddToRolesAsync(adminUser, new[] {
                    ApplicationRoles.Administrator,
                    ApplicationRoles.Moderator
                });
            }

            logger.Here().Information("Seed database associated with context {@DbContextName}", typeof(ApplicationDbContext).Name);
        }
    
        private async static Task SeedRoles(RoleManager<AppRole> roleManager)
        {
            var roles = new List<AppRole>
            {
                new AppRole{ Name = ApplicationRoles.Administrator },
                new AppRole{ Name = ApplicationRoles.Moderator },
                new AppRole{ Name = ApplicationRoles.Member }
            };

            foreach (var role in roles) await roleManager.CreateAsync(role);
        }
    }
}
