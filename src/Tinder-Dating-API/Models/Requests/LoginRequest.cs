using System.ComponentModel.DataAnnotations;

namespace Tinder_Dating_API.Models.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
