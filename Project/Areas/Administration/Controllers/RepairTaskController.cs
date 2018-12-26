using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common.Constants;
using Project.Models.Entities;
using Project.Models.ViewModels.Administration;
using Project.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Areas.Administration.Controllers
{
    public class RepairTaskController : BaseController
    {
        private readonly ITechnicianService technicianService;
        private readonly IRepairTaskService repairTaskService;
        private readonly IPartService partService;
        private readonly IMapper mapper;

        public RepairTaskController(ITechnicianService technicianService,
            IRepairTaskService repairTaskService,
            IPartService partService,
            IMapper mapper) {
            this.technicianService = technicianService;
            this.repairTaskService = repairTaskService;
            this.partService = partService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("/administration/[controller]/assign-repair-task/{id}")]
        public IActionResult AssignTask(int id) {
            RepairTask repairTask = this.repairTaskService.GetById(id);
            RepairTaskSimpleInfoViewModel repairTaskSimpleInfo = this.mapper.Map<RepairTaskSimpleInfoViewModel>(repairTask);
            this.partService.AllPartsForRepairTaskAreAvailable(repairTaskSimpleInfo);
            if(repairTaskSimpleInfo.CanBeAssigned == false || repairTask.Status != Models.Enums.Status.Pending) {
                TempData[StringConstants.TempDataKeyHoldingGenericError] = StringConstants.RepairTaskGenericAssignmentFailure;
                return this.RedirectToAction(StringConstants.ActionNameIndex, StringConstants.HomeControllerName);
            }
            AvailableTechniciansForRepairTaskViewModel availableTechnicians = new AvailableTechniciansForRepairTaskViewModel {
                TaskId = id,
                AvailableTechnicinsName = this.technicianService
                .GetAllAvailableTechnicians()
                .GetAwaiter()
                .GetResult()
                .Select(x => x.UserName)
                .ToArray()
            };
            return this.View(availableTechnicians);
        }

        [HttpPost]
        [Route("/administration/[controller]/assign-repair-task/{id}")]
        public async Task<IActionResult> AssignTask(AvailableTechniciansForRepairTaskViewModel availableTechniciansForRepairTaskViewModel, int id) {
            await this.technicianService.AddTechniciansToRepairTaskAsync(availableTechniciansForRepairTaskViewModel.SelectedTechnicians,
                id);

            return this.RedirectToAction(StringConstants.ActionNameIndex, StringConstants.HomeControllerName);
        }
    }
}
