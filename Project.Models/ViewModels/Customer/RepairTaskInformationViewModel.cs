using Project.Models.Enums;

namespace Project.Models.ViewModels.Customer
{
    public class RepairTaskInformationViewModel
    {
        public int Id { get; set; }

        public Status Status { get; set; }

        public decimal? Cost { get; set; }
    }
}
