using System.ComponentModel.DataAnnotations;

namespace ChallengeTiles.Server.Models.DTO
{
    public class RegisterPlayerRequest
    {
        [Required]
        [MaxLength(45)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
