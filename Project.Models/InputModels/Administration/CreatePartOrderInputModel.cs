using Project.Models.Attributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Project.Models.InputModels.Administration
{
    public class CreatePartOrderInputModel
    {
        [Display(Name = "Body Part")]
        [CheckThatOrderedAmountIsPositiveNumber(nameof(CarBodyPartAmount))]
        public bool IsCarBodyPart { get; set; }
        [Display(Name = "Amount")]
        public int CarBodyPartAmount { get; set; }

        [Display(Name = "Electronic part")]
        [CheckThatOrderedAmountIsPositiveNumber(nameof(ElectronicPartAmount))]
        public bool IsElectronicPart { get; set; }
        [Display(Name = "Amount")]
        public int ElectronicPartAmount { get; set; }

        [Display(Name = "Interior part")]
        [CheckThatOrderedAmountIsPositiveNumber(nameof(InteriorPartAmount))]
        public bool IsInteriorPart { get; set; }
        [Display(Name = "Amount")]
        public int InteriorPartAmount { get; set; }

        [Display(Name = "Chassis part")]
        [CheckThatOrderedAmountIsPositiveNumber(nameof(ChassisPartAmount))]
        public bool IsChassisPart { get; set; }
        [Display(Name = "Amount")]
        public int ChassisPartAmount { get; set; }
    }
}
