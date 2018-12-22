using Project.Models.Enums;
using System;
using System.Collections.Generic;

namespace Project.Models.Entities
{
    public class RepairTask
    {
        public RepairTask() {
            this.Technicians = new HashSet<UserRepairTask>();
            this.PartsRequired = new HashSet<Part>();
        }

        public int Id { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<UserRepairTask> Technicians { get; set; }

        public virtual ICollection<Part> PartsRequired { get; set; }

        public Status Status { get; set; }
    }
}
