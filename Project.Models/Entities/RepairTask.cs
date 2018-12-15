using Project.Models.Enums;
using System.Collections.Generic;

namespace Project.Models.Entities
{
    public class RepairTask
    {
        public int Id { get; set; }

        public virtual ICollection<UserRepairTask> Experts { get; set; }

        public Status Status { get; set; }
    }
}
