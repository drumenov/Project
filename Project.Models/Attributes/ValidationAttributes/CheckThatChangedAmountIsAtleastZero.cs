using Project.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Project.Models.Attributes.ValidationAttributes
{
    public class CheckThatChangedAmountIsAtleastZero : ValidationAttribute
    {
        private readonly string propertyName;

        public CheckThatChangedAmountIsAtleastZero(string propertyName) {
            this.propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            bool partOrdered = (bool)value;
            int amountOrdered = (int)validationContext
                .ObjectType
                .GetProperty(this.propertyName)
                .GetValue(validationContext.ObjectInstance);
            if (partOrdered && amountOrdered < 0) {
                return new ValidationResult(StringConstants.WrongAmountOfPartTypeToUpdate);
            }
            if (partOrdered == false && amountOrdered != 0) {
                return new ValidationResult(StringConstants.AmountForNotSelectedPartTypeToUpdateError);
            }
            return ValidationResult.Success;
        }
    }
}
