using Project.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Models.Attributes.ValidationAttributes
{
    public class AddRemoveTechnician : ValidationAttribute
    {
        private readonly string propertyName;

        public AddRemoveTechnician(string propertyName) {
            this.propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            HashSet<string> techcniciansToRemove = value as HashSet<string>;
            if(techcniciansToRemove.Count == 0) {
                HashSet<string> techniciansToAdd = validationContext.ObjectType.GetProperty(this.propertyName).GetValue(validationContext.ObjectInstance) as HashSet<string>;
                if (techniciansToAdd.Count == 0) {
                    return new ValidationResult(StringConstants.WrongInputWhenAddingOrRemovingTechniciansFromRepairTask);
                }
            }
            return ValidationResult.Success;
        }
    }
}
