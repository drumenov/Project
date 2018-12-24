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
    [ViewComponent(Name = StringConstants.ViewComponentCustomerWorkedOnRepairTasks)]
    public class CustomerWorkedOnRepairTasksViewComponent : ViewComponent
    {
        private readonly IRepairTaskService repairTaskService;
        private readonly IMapper mapper;

        public CustomerWorkedOnRepairTasksViewComponent(IRepairTaskService repairTaskService,
            IMapper mapper) {
            this.repairTaskService = repairTaskService;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page) {
            RepairTaskViewModel[] allWorkedOnRepairTasksPerCustomer = this.mapper
                .ProjectTo<RepairTaskViewModel>(await this.repairTaskService.GetWorkedOnPerCustomerAsync(this.User.Identity.Name))
                .ToArray();

            IPagedList<RepairTaskViewModel> workedOnRepairTasksPerCustomerToDisplay = allWorkedOnRepairTasksPerCustomer
                                                                                        .ToPagedList(page, IntegerConstants.ItemsPerPage);
            return this.View(workedOnRepairTasksPerCustomerToDisplay);
        }
    }
}
