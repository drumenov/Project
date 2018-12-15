using System.ComponentModel.DataAnnotations;

namespace Project.Models.InputModels
{
    public class LoginUserInputModel
    {
        [Required]
        [Display(Name = nameof(Username))]
        public string Username { get; set; }

        [Required]
        [Display(Name = nameof(Password))]
        [UIHint(nameof(Password))]
        public string Password { get; set; }
    }
}
