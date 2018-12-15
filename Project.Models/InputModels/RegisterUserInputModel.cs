using System.ComponentModel.DataAnnotations;

namespace Project.Models.InputModels
{
    public class RegisterUserInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [Compare(nameof(ConfirmPassword))]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Information { get; set; }
    }
}
