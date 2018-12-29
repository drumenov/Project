using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Customer.Controllers.Base;
using Project.Common.Constants;
using Project.Models.Entities;
using Project.Models.InputModels.Customer;
using Project.Models.ViewModels.Customer;
using Project.Services.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Project.Areas.Customer.Controllers
{
    public class RepairTaskController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly IRepairTaskService repairTaskService;
        private readonly IMapper mapper;

        public RepairTaskController(UserManager<User> userManager,
            IRepairTaskService repairTaskService,
            IMapper mapper) {
            this.userManager = userManager;
            this.repairTaskService = repairTaskService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("/customer/[controller]/create-repair-task")]
        public IActionResult CreateRepairTask() => this.View();

        [HttpPost]
        [Route("/customer/[controller]/create-repair-task")]
        public async Task<IActionResult> CreateRepairTask(RepairTaskInputModel repairTaskInputModel) {
            if (ModelState.IsValid == false) {
                return this.View(repairTaskInputModel);
            }
            if (repairTaskInputModel.IsCarBodyPart == false
                && repairTaskInputModel.IsChassisPart == false
                && repairTaskInputModel.IsElectronicPart == false
                && repairTaskInputModel.IsInteriorPart == false) {
                ModelState.AddModelError("", StringConstants.WrongRepairTask);
                return this.View(repairTaskInputModel);
            }
            User user = await this.userManager.FindByNameAsync(this.User.Identity.Name);
            int repairTaskId = await this.repairTaskService.CreateRepairTaskAsync(repairTaskInputModel, user);
            return this.RedirectToAction("repair-task-details", new { id = repairTaskId }); //TODO: Redirect to a detailed view of the order.
        } /*TODO: Check if all types of parts exists 
                                                                                                         in DB and only then create the RepairTask 
                                                                                                         object. Otherwise, the part is created and 
                                                                                                         added in the DB - unwanted behavior*/

        [HttpGet]
        [Route("customer/[controller]/repair-task-details/{id}")]
        public IActionResult RepairTaskDetails(int id) {
            RepairTask repairTask = this.repairTaskService?.GetById(id);
            if (repairTask == null) {
                throw new ArgumentNullException(String.Format(StringConstants.NoRepairTaskWithGivenIdError, id));
            }
            RepairTaskDetailsViewModel repairTaskViewModel = this.mapper.Map<RepairTaskDetailsViewModel>(repairTask);
            return this.View(repairTaskViewModel);
        }

        [HttpGet]
        [Route("/customer/[controller]/all-repair-tasks")]
        public IActionResult AllRepairTasks(int? page) {
            int currentPage = page ?? 1;
            RepairTaskInformationViewModel[] allCustomerRepairTasks = this.repairTaskService.GetAllPerCustomer(this.User.Identity.Name) != null
                ? this.mapper.ProjectTo<RepairTaskInformationViewModel>(this.repairTaskService.GetAllPerCustomer(this.User.Identity.Name)).ToArray()
                : new RepairTaskInformationViewModel[0];
            IPagedList repairTasksToDisplay = allCustomerRepairTasks.ToPagedList(currentPage, IntegerConstants.ItemsPerPageInViews);
            return this.View(repairTasksToDisplay);
        }
    }
}
