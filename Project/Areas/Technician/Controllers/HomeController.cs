using Microsoft.AspNetCore.Mvc;
using Project.Areas.Technician.Controllers.Base;
using Project.Common.Constants;

namespace Project.Areas.Technician.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index(int? workedOnRepairTasksByTechnicianPage,
            int? finishedRepairTasksByTechnicianPage) {
            int currentWorkedOnRepairTasksPage = workedOnRepairTasksByTechnicianPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageForWorkedOnRepairTasks] = currentWorkedOnRepairTasksPage;

            int currentFinishedRepairTasksByTechnician = finishedRepairTasksByTechnicianPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageForFinishedRepairTasksByTechnician] = currentFinishedRepairTasksByTechnician;

            return this.View();
        }
    }
}
