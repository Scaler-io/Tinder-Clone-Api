using System;

namespace Tinder_Dating_API.Models.Responses
{
    public class UserDetailsResponse 
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public UserProfileResponse Profile { get; set; }
    }
}
