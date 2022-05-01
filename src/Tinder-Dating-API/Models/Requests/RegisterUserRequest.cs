using System.ComponentModel.DataAnnotations;
using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.Models.Requests
{
    public class RegisterUserRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public UserProfileRequest Profile { get; set; }
    }

    public class UserProfileRequest
    {
        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public string KnownAs { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public UserAddressRequest Address { get; set; }
    }

    public class UserAddressRequest
    {
        public int UnitNumber { get; set; }
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int PostCode { get; set; }
    }
}
