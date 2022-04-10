using System.ComponentModel.DataAnnotations;

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
    }
}
