using System.Collections.Generic;

namespace Tinder_Dating_API.Models.Responses
{
    public class MemberProfileResponse
    {
        public int Age { get; set; }
        public string ImageUrl { get; set; }
        public string KnownAs { get; set; }      
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string AddressLine { get; set; }
        public string Created { get; set; }
        public string LastActive { get; set; }
        public ICollection<MemberImageResponse> Images { get; set; }
    }
}
