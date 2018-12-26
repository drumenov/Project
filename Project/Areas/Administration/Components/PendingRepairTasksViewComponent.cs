using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Constants;
using Project.Models.ViewModels.Administration;
using Project.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Project.Areas.Administration.Components
{
    [ViewComponent(Name = StringConstants.ViewComponentAllPendingRepairTasks)]
    public class PendingRepairTasksViewComponent : ViewComponent
    {
        private readonly IRepairTaskService repairTaskService;
        private readonly IPartService partService;
        private readonly IMapper mapper;

        public PendingRepairTasksViewComponent(IRepairTaskService repairTaskService,
            IPartService partService,
            IMapper mapper) {
            this.repairTaskService = repairTaskService;
            this.partService = partService;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page) {
            RepairTaskSimpleInfoViewModel[] allPendingRepairTasks = this.mapper
                .ProjectTo<RepairTaskSimpleInfoViewModel>(this.repairTaskService.GetAllPending())
                .ToArray();
            IPagedList<RepairTaskSimpleInfoViewModel> pendingRepairTasksToDisplayOnPage = allPendingRepairTasks
                                                                                            .ToPagedList(page, IntegerConstants.ItemsPerPage);
            foreach(RepairTaskSimpleInfoViewModel pendingRepairTask in pendingRepairTasksToDisplayOnPage) {
                this.partService.AllPartsForRepairTaskAreAvailable(pendingRepairTask);
                if(pendingRepairTask.CanBeAssigned == false) {

                }
            }
            return this.View(pendingRepairTasksToDisplayOnPage);
        }
    }
}
