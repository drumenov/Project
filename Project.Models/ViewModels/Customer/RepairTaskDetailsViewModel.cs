using Project.Models.Entities;
using Project.Models.Enums;
using System.Collections.Generic;

namespace Project.Models.ViewModels.Customer
{
    public class RepairTaskDetailsViewModel
    {
        public ICollection<Part> PartsRequired { get; set; }

        public ICollection<string> Technicians { get; set; } //This holds the username of the technicians, working on this task.

        public Status Status { get; set; }
    }
}
