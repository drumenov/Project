using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common.Constants;
using Project.Models.ViewModels.Administration;
using Project.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Areas.Administration.Controllers
{
    public class RepairTaskController : BaseController
    {
        private readonly ITechnicianService technicianService;

        public RepairTaskController(ITechnicianService technicianService,
            IMapper mapper) {
            this.technicianService = technicianService;
        }

        [HttpGet]
        [Route("/administration/[controller]/assign-repair-task/{id}")]
        public IActionResult AssignTask(int id) {
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
