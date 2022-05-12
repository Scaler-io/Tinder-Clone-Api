using System;

namespace Tinder_Dating_API.Models.Responses
{
    public class LikeResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public string PhotoUrl { get; set; }
        public string AddressLine { get; set; }
    }
}
