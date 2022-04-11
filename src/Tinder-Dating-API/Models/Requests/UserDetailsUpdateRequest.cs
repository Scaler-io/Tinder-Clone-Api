using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.Models.Requests
{
    public class UserDetailsUpdateRequest
    {
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public UserAddress Address { get; set; }
    }
}
