using System;

namespace Tinder_Dating_API.Entites
{
    public class UserAddress : BaseEntity
    {
        public int UnitNumber { get; set; }
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int PostCode { get; set; }

        public Guid UserProfileId { get; set; }
        public UserProfile Profile { get; set; }
    }

}