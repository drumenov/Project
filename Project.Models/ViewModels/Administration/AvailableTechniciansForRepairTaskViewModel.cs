using System.Collections.Generic;

namespace Project.Models.ViewModels.Administration
{
    public class AvailableTechniciansForRepairTaskViewModel
    {
        public int TaskId { get; set; }

        public ICollection<string> AvailableTechnicinsName { get; set; }

        public ICollection<string> SelectedTechnicians { get; set; }
    }
}
