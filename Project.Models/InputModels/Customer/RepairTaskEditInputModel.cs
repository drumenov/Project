using Project.Models.Attributes.ValidationAttributes;
using Project.Models.InputModels.BaseInputModels;

namespace Project.Models.InputModels.Customer
{
    public class RepairTaskEditInputModel : BaseModelForOrderingPartsAndRepairingTasks
    {
        public int Id { get; set; }

        [CheckThatChangedAmountIsAtleastZero(nameof(CarBodyPartAmount))]
        public override bool IsCarBodyPart { get => base.IsCarBodyPart; set => base.IsCarBodyPart = value; }

        [CheckThatChangedAmountIsAtleastZero(nameof(InteriorPartAmount))]
        public override bool IsInteriorPart { get => base.IsInteriorPart; set => base.IsInteriorPart = value; }

        [CheckThatChangedAmountIsAtleastZero(nameof(ElectronicPartAmount))]
        public override bool IsElectronicPart { get => base.IsElectronicPart; set => base.IsElectronicPart = value; }

        [CheckThatChangedAmountIsAtleastZero(nameof(ChassisPartAmount))]
        public override bool IsChassisPart { get => base.IsChassisPart; set => base.IsChassisPart = value; }
    }
}
