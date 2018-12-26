using Project.Models.Entities;
using System.Collections.Generic;

namespace Project.Models.ViewModels.Administration
{
    public class RepairTaskSimpleInfoViewModel
    {
        public RepairTaskSimpleInfoViewModel() {
            this.PartsRequired = new HashSet<Part>();
            this.PartsMissingMeesage = new List<string>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public ICollection<Part> PartsRequired { get; set; } //TODO: This is to be display as a dropdown item underneeth the order itself in the view.

        public bool CanBeAssigned { get; set; }

        public IList<string> PartsMissingMeesage { get; set; }
    }
}
