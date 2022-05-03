using Microsoft.AspNetCore.Identity;
using System;

namespace Tinder_Dating_API.Entites
{
    public class AppUserRole : IdentityUserRole<Guid>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}
