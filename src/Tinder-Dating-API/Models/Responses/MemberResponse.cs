using System;

namespace Tinder_Dating_API.Models.Responses
{
    public class MemberResponse 
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public MemberProfileResponse Profile { get; set; }
    }
}
