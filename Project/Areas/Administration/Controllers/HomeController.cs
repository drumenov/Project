using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common.Constants;

namespace Project.Areas.Administration.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index(int? adminsPage, int? customersPage, int? techniciansPage, int? pendingRepairTasks) {
            int currentAdminsPage = adminsPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageNumberForAdministrators] = currentAdminsPage;

            int currentCustomersPage = customersPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageNumberForCustomers] = currentCustomersPage;

            int currentTechniciansPage = techniciansPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageNumberForTechnicians] = currentTechniciansPage;

            int currentPendingRepairsTask = pendingRepairTasks ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageNumberForPendingRepairTasks] = currentPendingRepairsTask;

            return this.View();
        }
    }
    
}
