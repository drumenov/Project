using Microsoft.AspNetCore.Mvc;
using Project.Areas.Customer.Controllers.Base;
using Project.Common.Constants;

namespace Project.Areas.Customer.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index(int? pendingRepairTasksPerCustomerPage,
            int? workedOnRepairTasksPerCustomerPage,
            int? finishedRepairTasksPerCustomerPage) {

            int currentPendingRepairTasksPerCustomerPage = pendingRepairTasksPerCustomerPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageForPendingRepairTasksPerCustomer] = currentPendingRepairTasksPerCustomerPage;

            int currentWorkedOnRepairTasksPerCustomerPage = workedOnRepairTasksPerCustomerPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageForWorkedOnRepairTasksPerCustomer] = currentWorkedOnRepairTasksPerCustomerPage;

            int currentFinishedRepairTasksPerCustomerPage = finishedRepairTasksPerCustomerPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageforFinishedRepairTasksPerCustomer] = currentFinishedRepairTasksPerCustomerPage;

            return this.View(); //TODO: Think about using View Components here.
        }
    }
}
