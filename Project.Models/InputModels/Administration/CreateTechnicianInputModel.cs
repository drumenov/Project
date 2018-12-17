using Project.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Models.InputModels.Administration
{
    public class CreateTechnicianInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public TechnicianLevel Level { get; set; }

        [Required]
        [UIHint(nameof(Password))]
        [Compare(nameof(ConfirmPassword))]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [UIHint(nameof(Password))]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
