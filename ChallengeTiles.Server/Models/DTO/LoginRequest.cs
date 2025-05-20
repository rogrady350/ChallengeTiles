using System.ComponentModel.DataAnnotations;

namespace ChallengeTiles.Server.Models.DTO
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
