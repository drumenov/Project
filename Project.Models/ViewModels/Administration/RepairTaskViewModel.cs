using Project.Models.ViewModels.BaseViewModels;

namespace Project.Models.ViewModels.Administration
{
    public class RepairTaskViewModel : BaseRepairTaskViewModel
    {
        public string Username { get; set; }

        public int CountOfTechniciansCurrentlyWorkingOnTheRepairTask { get; set; }

        public int CountOfTechniciansHavingFinishedWorkingOnTheRepairTask { get; set; }

        public bool NoTechniciansWorking => CountOfTechniciansCurrentlyWorkingOnTheRepairTask == 0;

        public bool NoTechnicianHasFinishedWrokingOnTheRepairTask => CountOfTechniciansHavingFinishedWorkingOnTheRepairTask == 0;
    }
}
