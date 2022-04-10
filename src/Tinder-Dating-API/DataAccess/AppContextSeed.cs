using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
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
                //logger.Information($"User data fetched. {JsonSerializer.Serialize(users)}");

                foreach(var user in users)
                {
                    user.UserName = user.UserName.ToLower();
                    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("P@ssw0rd"));
                    user.PasswordSalt = hmac.Key;

                    context.Users.Add(user);
                }

                await context.SaveChangesAsync();
                //var user = new AppUser(Guid.NewGuid(), "Jane");

                //var profile = new UserProfile
                //{
                //    Id = Guid.NewGuid(),
                //    Gender = "female",
                //    DateOfBirth = new DateTime(1997, 04, 20),
                //    KnownAs = user.UserName,
                //    Created = new DateTime(2019, 04, 20),
                //    LastActive = new DateTime(2022, 05, 16),
                //    Introduction = "",
                //    Interests = "",
                //    LookingFor = "",
                //    Address = new UserAddress
                //    {
                //        Id = Guid.NewGuid(),
                //        UnitNumber = 0,
                //        StreetNumber = 0,
                //        StreetName = "Paticolony",
                //        StreetType = "Road",
                //        City = "Siliguri",
                //        State = "West Bengal",
                //        PostCode = 734003
                //    },
                //    Images = new List<UserImage>
                //    {
                //       new UserImage
                //        {
                //            Id = Guid.NewGuid(),
                //            Url = "https://randomuser.me/api/portraits/women/35.jpg",
                //            IsMain = true
                //        }
                //    }
                //};

                //var hmac = new HMACSHA512();
                //user.Profile = profile;
                //user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("P@ssw0rd"));
                //user.PasswordSalt = hmac.Key;

                //context.Users.Add(user);

                //await context.SaveChangesAsync();
            }

            logger.Here().Information("Seed database associated with context {@DbContextName}", typeof(ApplicationDbContext).Name);
        }
    }
}
