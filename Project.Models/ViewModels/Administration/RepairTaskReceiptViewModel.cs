using System.Collections.Generic;

namespace Project.Models.ViewModels.Administration
{
    public class RepairTaskReceiptViewModel
    {
        public int ReceiptId { get; set; }

        public string CustomerName { get; set; }

        public decimal TotalRevenue { get; set; }

        public ICollection<string> NamesOfTechniciansHavingWorkedOnTheRepairTask { get; set; }
    }
}
