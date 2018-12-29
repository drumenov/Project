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
    [ViewComponent(Name = StringConstants.ViewComponentCustomerFinishedRepairTasks)]
    public class CustomerFinishedRepairTasksViewComponent : ViewComponent
    {
        private readonly IRepairTaskService repairTaskService;
        private readonly IMapper mapper;

        public CustomerFinishedRepairTasksViewComponent(IRepairTaskService repairTaskService,
            IMapper mapper) {
            this.repairTaskService = repairTaskService;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page) {
            RepairTaskViewModel[] allFinishedRepairTasksPerCustomer = this.mapper
                .ProjectTo<RepairTaskViewModel>(await this.repairTaskService.GetFinishedPerCustomerAsync(this.User.Identity.Name))
                .ToArray();

            IPagedList<RepairTaskViewModel> finishedRepairTasksPerCustomerToDisplay = allFinishedRepairTasksPerCustomer
                                                                                        .ToPagedList(page, IntegerConstants.ItemsPerPageInViewComponents);
            return this.View(finishedRepairTasksPerCustomerToDisplay);
        }
    }
}
