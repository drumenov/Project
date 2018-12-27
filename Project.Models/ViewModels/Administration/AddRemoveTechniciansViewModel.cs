using Project.Models.Attributes.ValidationAttributes;
using System.Collections.Generic;

namespace Project.Models.ViewModels.Administration
{
    public class AddRemoveTechniciansViewModel
    {
        public AddRemoveTechniciansViewModel() {
            this.TechniciansWorkingOnRepairTask = new HashSet<string>();
            this.AvailableTechnicians = new HashSet<string>();
            this.TechniciansToRemove = new HashSet<string>();
            this.TechniciansToAdd = new HashSet<string>();
        }

        public int Id { get; set; }

        public ICollection<string> TechniciansWorkingOnRepairTask { get; set; }

        public ICollection<string> AvailableTechnicians { get; set; }
        [AddRemoveTechnician(nameof(TechniciansToAdd))] //Checks that at least one technician is removed or added
        public ICollection<string> TechniciansToRemove { get; set; }

        public ICollection<string> TechniciansToAdd { get; set; }
    }
}
