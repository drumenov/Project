using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common.Constants;

namespace Project.Areas.Administration.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index(int? adminsPage,
            int? customersPage,
            int? techniciansPage,
            int? pendingRepairTasksPage,
            int? workedOnRepairTasksPage,
            int? finishedRepairTasksPage) {

            int currentAdminsPage = adminsPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageNumberForAdministrators] = currentAdminsPage;

            int currentCustomersPage = customersPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageNumberForCustomers] = currentCustomersPage;

            int currentTechniciansPage = techniciansPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageNumberForTechnicians] = currentTechniciansPage;

            int currentPendingRepairsTask = pendingRepairTasksPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageNumberForPendingRepairTasks] = currentPendingRepairsTask;

            int currentPageWorkedOnRepairTasksPage = workedOnRepairTasksPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageForWorkedOnRepairTasks] = currentPageWorkedOnRepairTasksPage;

            int currentPageFinishedRepairTasks = finishedRepairTasksPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageForFinishedRepairTasks] = currentPageFinishedRepairTasks;
            return this.View();
        }
    }
    
}
