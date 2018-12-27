using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Technician.Controllers.Base;
using Project.Models.Entities;
using Project.Models.ViewModels.Technician;
using Project.Services.Contracts;
using System.Threading.Tasks;

namespace Project.Areas.Technician.Controllers
{
    public class RepairTaskController : BaseController
    {
        private readonly IRepairTaskService repairTaskService;
        private readonly IMapper mapper;

        public RepairTaskController(IRepairTaskService repairTaskService,
            IMapper mapper) {
            this.repairTaskService = repairTaskService;
            this.mapper = mapper;
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

        [HttpGet]
        [Route("/technician/[controller]/repair-task-details/{id}")]
        public IActionResult RepairTaskDetails(int id) {
            RepairTask repairTask = this.repairTaskService.GetById(id);
            RepairTaskDetailsViewModel repairTaskDetailsViewModel = this.mapper.Map<RepairTaskDetailsViewModel>(repairTask);
            return this.View(repairTaskDetailsViewModel);
        }
    }
}
