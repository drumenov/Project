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
    [ViewComponent(Name = StringConstants.ViewComponentFinishedRepairTasks)]
    public class FinishedRepairTaskViewComponent : ViewComponent
    {
        private readonly IRepairTaskService repairTaskService;
        private readonly IMapper mapper;

        public FinishedRepairTaskViewComponent(IRepairTaskService repairTaskService,
            IMapper mapper) {
            this.repairTaskService = repairTaskService;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page) {
            RepairTaskViewModel[] allFinishedRepairTasks = this.mapper
                                                                .ProjectTo<RepairTaskViewModel>(this.repairTaskService
                                                                                                        .GetAllFinishedRepairTasks())
                                                                .ToArray();
            IPagedList<RepairTaskViewModel> finishedRepairTasksToDisplay = allFinishedRepairTasks.ToPagedList(page, IntegerConstants.ItemsPerPage);
            return this.View(finishedRepairTasksToDisplay);
        }

    }
}
