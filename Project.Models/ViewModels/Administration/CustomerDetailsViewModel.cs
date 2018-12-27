namespace Project.Models.ViewModels.Administration
{
    public class CustomerDetailsViewModel
    {
        public string CustomerName { get; set; }

        public int CountOfPendingRepairTasks { get; set; }

        public int CountOfWorkedOnRepairTasks { get; set; }

        public int CountOfFinishedRepairTasks { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}
