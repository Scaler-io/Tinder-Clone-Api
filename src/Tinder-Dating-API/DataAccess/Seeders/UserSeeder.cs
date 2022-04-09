using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.DataAccess.Seeders
{
    public class UserSeeder
    {
        public static List<AppUser> Users()
        {
            return new List<AppUser> {
                new AppUser(Guid.NewGuid(), "Bob"),
                new AppUser(Guid.NewGuid(), "Tom"),
                new AppUser(Guid.NewGuid(), "Jane"),
            };
        }
    }
}
