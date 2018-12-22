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
    [ViewComponent(Name = StringConstants.ViewComponentPendingRepairTasks)]
    public class PendingRepairTasksViewComponent : ViewComponent
    {
        private readonly IRepairTaskService repairTaskService;
        private readonly IMapper mapper;

        public PendingRepairTasksViewComponent(IRepairTaskService repairTaskService,
            IMapper mapper) {
            this.repairTaskService = repairTaskService;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page) {
            RepairTaskSimpleInfoViewModel[] allPendingRepairTasks = this.mapper
                .ProjectTo<RepairTaskSimpleInfoViewModel>(this.repairTaskService.GetAllPending())
                .ToArray();
            IPagedList<RepairTaskSimpleInfoViewModel> pendingRepairTasksToDisplayOnPage = allPendingRepairTasks
                                                                                            .ToPagedList(page, IntegerConstants.ItemsPerPage);
            return this.View(pendingRepairTasksToDisplayOnPage);
        }
    }
}
