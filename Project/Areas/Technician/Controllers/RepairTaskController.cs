using Microsoft.AspNetCore.Mvc;
using Project.Areas.Technician.Controllers.Base;
using Project.Services.Contracts;
using System.Threading.Tasks;

namespace Project.Areas.Technician.Controllers
{
    public class RepairTaskController : BaseController
    {
        private readonly IRepairTaskService repairTaskService;

        public RepairTaskController(IRepairTaskService repairTaskService) {
            this.repairTaskService = repairTaskService;
        }

        [HttpPost]
        [Route("/technician/[controller]/technician-does-magic/{id}")]
        public async Task<IActionResult> DoMagic(int id) {
            await this.repairTaskService.TechnicianCompletesARepairTaskAsync(id, this.User.Identity.Name);
            return this.RedirectToAction(nameof(this.RepairTaskFinishedByTechnician), new { id });
        }

        [HttpGet]
        [Route("technician/[controller]/repair-task-completed/{id}")]
        public IActionResult RepairTaskFinishedByTechnician(int id) {
            return this.View(id);
        }
    }
}
