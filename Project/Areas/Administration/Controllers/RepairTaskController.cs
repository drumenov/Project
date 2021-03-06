﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common.Constants;
using Project.Models.Entities;
using Project.Models.ViewModels.Administration;
using Project.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Areas.Administration.Controllers
{
    public class RepairTaskController : BaseController
    {
        private readonly ITechnicianService technicianService;
        private readonly IRepairTaskService repairTaskService;
        private readonly IPartService partService;
        private readonly IFeedbackService feedbackService;
        private readonly IMapper mapper;

        public RepairTaskController(ITechnicianService technicianService,
            IRepairTaskService repairTaskService,
            IPartService partService,
            IFeedbackService feedbackService,
            IMapper mapper) {
            this.technicianService = technicianService;
            this.repairTaskService = repairTaskService;
            this.partService = partService;
            this.feedbackService = feedbackService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("/administration/[controller]/assign-repair-task/{id}")]
        public IActionResult AssignTask(int id) {
            RepairTask repairTask = this.repairTaskService.GetById(id);
            RepairTaskSimpleInfoViewModel repairTaskSimpleInfo = this.mapper.Map<RepairTaskSimpleInfoViewModel>(repairTask);
            this.partService.AllPartsForRepairTaskAreAvailable(repairTaskSimpleInfo);
            if(repairTaskSimpleInfo.CanBeAssigned == false || repairTask.Status != Models.Enums.Status.Pending) {
                TempData[StringConstants.TempDataKeyHoldingGenericErrorsForRepairTaskController] = StringConstants.RepairTaskGenericAssignmentFailure;
                return this.RedirectToAction(StringConstants.ActionNameIndex, StringConstants.HomeControllerName);
            }
            AvailableTechniciansForRepairTaskViewModel availableTechnicians = new AvailableTechniciansForRepairTaskViewModel {
                TaskId = id,
                AvailableTechnicinsName = this.technicianService
                .GetAllAvailableTechnicians()
                .GetAwaiter()
                .GetResult()
                .Select(technician => technician.UserName)
                .ToArray()
            };
            return this.View(availableTechnicians);
        }

        [HttpPost]
        [Route("/administration/[controller]/assign-repair-task/{id}")]
        public async Task<IActionResult> AssignTask(AvailableTechniciansForRepairTaskViewModel availableTechniciansForRepairTaskViewModel, int id) {
            if(ModelState.IsValid == false) {
                availableTechniciansForRepairTaskViewModel.AvailableTechnicinsName = this.technicianService
                                                                                            .GetAllAvailableTechnicians()
                                                                                            .GetAwaiter()
                                                                                            .GetResult()
                                                                                            .Select(technician => technician.UserName)
                                                                                            .ToArray();
                return this.View(availableTechniciansForRepairTaskViewModel);
            }
            
            RepairTask repairTask = this.repairTaskService.GetById(id);
            RepairTaskSimpleInfoViewModel repairTaskSimpleInfo = this.mapper.Map<RepairTaskSimpleInfoViewModel>(repairTask);
            this.partService.AllPartsForRepairTaskAreAvailable(repairTaskSimpleInfo);
            if (repairTaskSimpleInfo.CanBeAssigned == false || repairTask.Status != Models.Enums.Status.Pending) {
                TempData[StringConstants.TempDataKeyHoldingGenericErrorsForRepairTaskController] = StringConstants.RepairTaskGenericAssignmentFailure;
                return this.RedirectToAction(StringConstants.ActionNameIndex, StringConstants.HomeControllerName);
            }
            await this.technicianService.AddTechniciansToRepairTaskAsync(availableTechniciansForRepairTaskViewModel.SelectedTechnicians,
                id);
            return this.RedirectToAction(StringConstants.ActionNameRepairTaskDetails, new { id });
        }

        [HttpGet]
        [Route("/administration/[controller]/repair-task-details/{id}")]
        public IActionResult RepairTaskDetails(int id) {
            RepairTask repairTask = this.repairTaskService.GetById(id);
            RepairTaskDetailsViewModel repairTaskDetailsViewModel = this.mapper.Map<RepairTaskDetailsViewModel>(repairTask);
            repairTaskDetailsViewModel.Feedback = this.feedbackService.GetByRepairTaskId(id)?.Content;
            return this.View(repairTaskDetailsViewModel);
        }

        [HttpGet]
        [Route("/administration/[controller]/add-or-remove-technicians/{id}")]
        public IActionResult AddRemoveTechnicians(int id) {
            AddRemoveTechniciansViewModel addRemoveTechniciansViewModel = new AddRemoveTechniciansViewModel {
                Id = id,
                AvailableTechnicians = this.technicianService.GetAllNamesOfTechniciansNotWorkingOnAGivenTask(id).GetAwaiter().GetResult().ToArray(),
                TechniciansWorkingOnRepairTask = this.technicianService.GetAllNamesOfTechniciansWorkingOnAGivenTask(id).ToArray()
            };            
            return this.View(addRemoveTechniciansViewModel);
        }

        [HttpPost]
        [Route("/administration/[controller]/add-or-remove-technicians/{id}")]
        public async Task<IActionResult> AddRemoveTechnicians(AddRemoveTechniciansViewModel addRemoveTechniciansViewModel, int id) {
            if(ModelState.IsValid == false) {
                return this.View(addRemoveTechniciansViewModel);
            }
            foreach (string nameOfTechnicianToAdd in addRemoveTechniciansViewModel.TechniciansToAdd) { /*Here the order is VERY IMPORTANT. Adding of technicians 
                                                                                                        must be the first operation since the removing of 
                                                                                                        technicians checks whether there are any technicians 
                                                                                                        left to work on the task. If there are none, the status 
                                                                                                        of the task is changed to PENDING. Hence, if the adding 
                                                                                                        is done after the removing and the changing of the 
                                                                                                        status there is a conflict (we have a PENDING Repair Task
                                                                                                        that has a Technician assigned to it).*/
                await this.repairTaskService.AddTechnicianToRepairTaskAsync(nameOfTechnicianToAdd, id);
            }
            foreach (string nameOfTechnicianToRemove in addRemoveTechniciansViewModel.TechniciansToRemove) {
                await this.repairTaskService.RemoveTechnicianFromRepairTaskAsync(nameOfTechnicianToRemove, id);
            }
            return this.RedirectToAction(StringConstants.ActionNameRepairTaskDetails, new { id });
        }

        [HttpGet]
        [Route("/administration/[controller]/repair-task-receipt/{id}")]
        public IActionResult RepairTaskReceipt(int id) {
            RepairTaskReceiptViewModel repairTaskReceiptViewModel = this.mapper.Map<RepairTaskReceiptViewModel>(this.repairTaskService.GetById(id));
            return this.View(repairTaskReceiptViewModel);
        }
    }
}
