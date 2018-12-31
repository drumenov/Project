using Project.Models.ViewModels.BaseViewModels;

namespace Project.Models.ViewModels.Customer
{
    public class RepairTaskViewModel : BaseRepairTaskViewModel
    {
        public bool CanCreateFeedback { get; set; }

        public bool CanEditFeedback { get; set; }
    }
}
