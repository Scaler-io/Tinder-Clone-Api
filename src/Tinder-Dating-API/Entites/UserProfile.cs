using System;
using System.Collections.Generic;
using Tinder_Dating_API.Extensions.Utilities;

namespace Tinder_Dating_API.Entites
{
    public class UserProfile : BaseEntity
    {
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public UserAddress Address { get; set; }
        public ICollection<UserImage> Images { get; set; } = new List<UserImage>();
        
        public Guid AppUserId { get; set; }
        public AppUser User { get; set; }

        public int GetAge()
        {
            return DateOfBirth.CalulateUserAge();
        }
    }
}
