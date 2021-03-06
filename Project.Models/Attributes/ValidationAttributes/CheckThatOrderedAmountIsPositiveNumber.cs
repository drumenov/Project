﻿using Project.Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Project.Models.Attributes.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class CheckThatOrderedAmountIsPositiveNumber : ValidationAttribute /*The role of this attribute is to check whether a type of part is ordered
                                                                      and if TRUE checks that the ordered amount is greater that zero.*/
    {
        private readonly string propertyName;

        public CheckThatOrderedAmountIsPositiveNumber(string propertyName) {
            this.propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            bool partOrdered = (bool)value;
            int amountOrdered = (int)validationContext
                .ObjectType
                .GetProperty(this.propertyName)
                .GetValue(validationContext.ObjectInstance);
            if(partOrdered && amountOrdered <= 0) {
                return new ValidationResult(StringConstants.WrongAmountOfOrderedPartSelected);
            }
            if(partOrdered == false && amountOrdered != 0) {
                return new ValidationResult(StringConstants.WrongAmountOfUnorderedPartSelected);
            }
            return ValidationResult.Success;
        }
    }
}
