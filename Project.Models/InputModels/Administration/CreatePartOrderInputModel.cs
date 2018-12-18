using Project.Models.Attributes.ValidationAttributes;

namespace Project.Models.InputModels.Administration
{
    public class CreatePartOrderInputModel
    {

        [CheckThatOrderedAmountIsPositiveNumber(nameof(Amount))]
        public bool TypeOfPart { get; set; }

        public int Amount { get; set; }
    }
}
