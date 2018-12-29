using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Models.ViewModels.Administration;
using Project.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Areas.Administration.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IRepairTaskService repairTaskService;
        private readonly IReceiptService receiptService;

        public CustomerController(IRepairTaskService repairTaskService,
            IReceiptService receiptService) {
            this.repairTaskService = repairTaskService;
            this.receiptService = receiptService;
        }

        [HttpGet]
        [Route("/administration/[controller]/customer-details/{customerName}")]
        public IActionResult CustomerDetails(string customerName) {
            CustomerDetailsViewModel customerDetailsViewModel = new CustomerDetailsViewModel {
                CountOfFinishedRepairTasks = this.repairTaskService
                                                    .GetFinishedPerCustomerAsync(customerName)
                                                    .GetAwaiter()
                                                    .GetResult()
                                                    .Count(),
                CountOfPendingRepairTasks = this.repairTaskService
                                                    .GetPendingPerCustomerAsync(customerName)
                                                    .GetAwaiter()
                                                    .GetResult()
                                                    .Count(),
                CountOfWorkedOnRepairTasks = this.repairTaskService
                                                    .GetWorkedOnPerCustomerAsync(customerName)
                                                    .GetAwaiter()
                                                    .GetResult()
                                                    .Count(),
                CustomerName = customerName,
                TotalRevenue = this.receiptService.GetTotalRevenuePerCustomer(customerName)
            };
            return this.View(customerDetailsViewModel);
        }
    }
}
