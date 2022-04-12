using System;
using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.Models.Requests
{
    public class UserDetailsUpdateRequest
    {
        public string KnownAs { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public UserAddress Address { get; set; }
    }
}
