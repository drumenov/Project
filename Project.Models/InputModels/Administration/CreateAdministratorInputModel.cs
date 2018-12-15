using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Models.InputModels.Administration
{
    public class CreateAdministratorInputModel
    {
        [Required]
        [UIHint(nameof(Username))]
        public string Username { get; set; }

        [Required]
        [UIHint(nameof(Password))]
        [Compare(nameof(ConfirmPassword))]
        public string Password { get; set; }

        [Required]
        [UIHint(nameof(Password))]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }        
    }
}
