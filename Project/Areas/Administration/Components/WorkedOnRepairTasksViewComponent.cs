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
    [ViewComponent(Name = StringConstants.ViewComponentWorkedOnRepairTasks)]
    public class WorkedOnRepairTasksViewComponent : ViewComponent
    {
        private readonly IRepairTaskService repairTaskService;
        private readonly IMapper mapper;

        public WorkedOnRepairTasksViewComponent(IRepairTaskService repairTaskService,
            IMapper mapper) {
            this.repairTaskService = repairTaskService;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page) {
            RepairTaskViewModel[] allWorkedOnRepairTasks = this.mapper
                                                                .ProjectTo<RepairTaskViewModel>(this.repairTaskService
                                                                                                    .GetAllWorkedOn())
                                                                                                    .ToArray();
            IPagedList<RepairTaskViewModel> repairTasksWorkedOnToDisplay = allWorkedOnRepairTasks.ToPagedList(page, IntegerConstants.ItemsPerPage);
            return this.View(repairTasksWorkedOnToDisplay);
        }
    }
}
