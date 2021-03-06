﻿using Project.Models.Attributes.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Models.InputModels.BaseInputModels
{
    public class BaseModelForOrderingPartsAndRepairingTasks
    {
        [Display(Name = "Body Part")]
        [CheckThatOrderedAmountIsPositiveNumber(nameof(CarBodyPartAmount))]
        public virtual bool IsCarBodyPart { get; set; }
        [Display(Name = "Amount")]
        public int CarBodyPartAmount { get; set; }

        [Display(Name = "Electronic part")]
        [CheckThatOrderedAmountIsPositiveNumber(nameof(ElectronicPartAmount))]
        public virtual bool IsElectronicPart { get; set; }
        [Display(Name = "Amount")]
        public int ElectronicPartAmount { get; set; }

        [Display(Name = "Interior part")]
        [CheckThatOrderedAmountIsPositiveNumber(nameof(InteriorPartAmount))]
        public virtual bool IsInteriorPart { get; set; }
        [Display(Name = "Amount")]
        public int InteriorPartAmount { get; set; }

        [Display(Name = "Chassis part")]
        [CheckThatOrderedAmountIsPositiveNumber(nameof(ChassisPartAmount))]
        public virtual bool IsChassisPart { get; set; }
        [Display(Name = "Amount")]
        public int ChassisPartAmount { get; set; }
    }
}
