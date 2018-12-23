using Microsoft.AspNetCore.Mvc;
using Project.Areas.Technician.Controllers.Base;
using Project.Common.Constants;

namespace Project.Areas.Technician.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index(int? workedOnRepairTasksPage) {
            int currentWorkedOnRepairTasksPage = workedOnRepairTasksPage ?? 1;
            TempData[StringConstants.TempDataKeyHoldingThePageForWorkedOnRepairTasks] = currentWorkedOnRepairTasksPage;


            return this.View();
        }
    }
}
