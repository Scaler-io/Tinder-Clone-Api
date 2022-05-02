using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tinder_Dating_API.Entites;
using Tinder_Dating_API.Extensions;

namespace Tinder_Dating_API.DataAccess
{
    public class AppContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
        {
            if(!await context.Users.AnyAsync())
            {

                var userData = File.ReadAllText("./DataAccess/Seeders/Users.json");
                var users = JsonConvert.DeserializeObject<List<AppUser>>(userData);
                var hmac = new HMACSHA512();
                
                
                foreach(var user in users)
                {
                    user.UserName = user.UserName.ToLower();
                    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("P@ssw0rd"));
                    user.PasswordSalt = hmac.Key;

                    context.Users.Add(user);
                }

                await context.SaveChangesAsync();
                
            }

            logger.Here().Information("Seed database associated with context {@DbContextName}", typeof(ApplicationDbContext).Name);
        }
    }
}
