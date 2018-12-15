﻿using System.ComponentModel.DataAnnotations;

namespace Project.Models.InputModels.Administration
{
    public class CreateCustomerInputModel
    {
        [Required]
        [Display(Name = "Client Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Is Corporate Client")]
        public bool IsCorporateClient { get; set; }

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
