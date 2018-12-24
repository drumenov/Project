using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Constants;
using Project.Models.ViewModels.Customer;
using Project.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Project.Areas.Customer.Components
{
    [ViewComponent(Name = StringConstants.ViewComponentCustomerPendingRepairTasks)]
    public class CustomerPendingRepairTasksViewComponent : ViewComponent
    {
        private readonly IRepairTaskService repairTaskService;
        private readonly IMapper mapper;

        public CustomerPendingRepairTasksViewComponent(IRepairTaskService repairTaskService,
            IMapper mapper) {
            this.repairTaskService = repairTaskService;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page) {
            RepairTaskViewModel[] allPendingRepairTaskPerCusotmer = this.mapper
                .ProjectTo<RepairTaskViewModel>(await this.repairTaskService.GetPendingPerCustomerAsync(this.User.Identity.Name))
                .ToArray();
            IPagedList<RepairTaskViewModel> pendingRepairTasksPerCustomersToDisplay = allPendingRepairTaskPerCusotmer
                                                                                        .ToPagedList(page, IntegerConstants.ItemsPerPage);
            return this.View(pendingRepairTasksPerCustomersToDisplay);
        }
    }
}
