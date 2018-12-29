using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Constants;
using Project.Models.Entities;
using Project.Models.ViewModels.Technician;
using Project.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Project.Areas.Technician.Components
{
    [ViewComponent(Name = StringConstants.ViewComponentWorkedOnRepairsTaskByTechnician)]
    public class WorkedOnRepairTasksViewComponent : ViewComponent
    {
        private readonly UserManager<User> userManager;
        private readonly IRepairTaskService repairTaskService;
        private readonly IMapper mapper;

        public WorkedOnRepairTasksViewComponent(UserManager<User> userManager,
            IRepairTaskService repairTaskService,
            IMapper mapper) {
            this.userManager = userManager;
            this.repairTaskService = repairTaskService;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page) {
            RepairTaskViewModel[] repairTaskViewModel = this.mapper
                .ProjectTo<RepairTaskViewModel>(this.repairTaskService.GetAllWorkedByATechnician(this.User.Identity.Name))
                .ToArray();
            IPagedList<RepairTaskViewModel> repairTasksToDisplay = repairTaskViewModel.ToPagedList(page, IntegerConstants.ItemsPerPageInViewComponents);
            return this.View(repairTasksToDisplay);
        }
    }
}
