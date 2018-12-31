using Project.Models.InputModels.BaseInputModels;

namespace Project.Models.InputModels.Customer
{
    public class RepairTaskEditInputModel : BaseModelForOrderingPartsAndRepairingTasks
    {
        public int Id { get; set; }

        public override bool IsCarBodyPart => base.CarBodyPartAmount > 0;

        public override bool IsChassisPart => base.ChassisPartAmount > 0;

        public override bool IsElectronicPart => base.ElectronicPartAmount > 0;

        public override bool IsInteriorPart => base.InteriorPartAmount > 0;
    }
}
