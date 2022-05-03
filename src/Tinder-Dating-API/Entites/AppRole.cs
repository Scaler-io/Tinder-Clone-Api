using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Tinder_Dating_API.Entites
{
    public class AppRole : IdentityRole<Guid>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
