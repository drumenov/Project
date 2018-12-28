using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models.Entities
{
    public class Receipt
    {
        public Receipt() {
            this.ExpertsHavingWorkedOnRepairTask = new HashSet<ExpertReceipt>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<ExpertReceipt> ExpertsHavingWorkedOnRepairTask { get; set; }

        public decimal TotalPrice { get; set; }
        
        public virtual RepairTask RepairTask { get; set; }
    }
}
