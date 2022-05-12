using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Tinder_Dating_API.Entites
{
    public class AppUser : IdentityUser<Guid>
    {
       
        public UserProfile Profile { get; set; } = new UserProfile();
        public ICollection<AppUserRole> UserRoles { get; set; }
        public AppUser() { }

        public AppUser(Guid id, string userName)
        {
            Id = id;
            UserName = userName;
        }

        public ICollection<UserLike> LikedByUser { get; set; }
        public ICollection<UserLike> LikedUsers { get; set; }
    }
}
