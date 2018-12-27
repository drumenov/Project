using Project.Models.Entities;
using System.Collections.Generic;

namespace Project.Models.ViewModels.Technician
{
    public class RepairTaskDetailsViewModel
    {
        public string Username { get; set; }

        public ICollection<Part> PartsRequired { get; set; }

        public ICollection<User> TechniciansWorkingOnRepairTask { get; set; }
    }
}
