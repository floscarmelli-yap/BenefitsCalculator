using System.ComponentModel.DataAnnotations;

namespace BenefitsCalculator.Models
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
