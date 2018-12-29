using System.Collections.Generic;

namespace Project.Models.ViewModels.Administration
{
    public class ReceiptDetailsViewModel
    {
        public int ReceiptId { get; set; }

        public int RepairTaskId { get; set; }

        public string CustomerName { get; set; }

        public decimal TotalRevenue { get; set; }

        public ICollection<string> NamesOfTechniciansHavingWorkedOnTheRepairTask { get; set; }
    }
}
