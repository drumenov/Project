using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Models.ViewModels.Administration
{
    public class AvailableTechniciansForRepairTaskViewModel
    {
        public int TaskId { get; set; }

        public ICollection<string> AvailableTechnicinsName { get; set; }

        [Required(ErrorMessage = " You must select at least one technician.")]
        public ICollection<string> SelectedTechnicians { get; set; }
    }
}
