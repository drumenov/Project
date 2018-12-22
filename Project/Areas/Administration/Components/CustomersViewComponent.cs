using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Constants;
using Project.Models.ViewModels.Administration;
using Project.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Project.Areas.Administration.Components
{
    [ViewComponent(Name = StringConstants.ViewComponentCustomersName)]
    public class CustomersViewComponent : ViewComponent
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public CustomersViewComponent(IUserService userService,
            IMapper mapper) {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? page) {
            try {
                int pageNumber = page ?? 1;
                CustomerViewModel[] allNonCorporateCustomers = await this.userService.GetAllUsersWithAGivenRoleAsync(StringConstants.CustomerUserRole) != null
                ? this.mapper.ProjectTo<CustomerViewModel>(await this.userService.GetAllUsersWithAGivenRoleAsync(StringConstants.CustomerUserRole)).ToArray()
                : new CustomerViewModel[0];

                CustomerViewModel[] allCorporateCustomers = await this.userService.GetAllUsersWithAGivenRoleAsync(StringConstants.CorporateCustomerUserRole) != null
                    ? this.mapper.ProjectTo<CustomerViewModel>(await this.userService.GetAllUsersWithAGivenRoleAsync(StringConstants.CorporateCustomerUserRole)).ToArray()
                    : new CustomerViewModel[0];

                IEnumerable<CustomerViewModel> allCustomers = allCorporateCustomers.Union(allNonCorporateCustomers);

                //int maximumNumberOfPages = allCustomers.Count() / IntegerConstants.ItemsPerPage;
                //if (allCustomers.Count() > IntegerConstants.ItemsPerPage && allCustomers.Count() % IntegerConstants.ItemsPerPage != 0) {
                //    maximumNumberOfPages += 1;
                //}
                //if (pageNumber <= 0 || pageNumber > maximumNumberOfPages) { //This check for the feasibility of the page number. If it is "out of range" the default value of 1 is selected."
                //    pageNumber = 1;
                //}
                //TempData[StringConstants.TempDataKeyHoldingNumberOfMaximumPagesForCustomers] = maximumNumberOfPages;
                IPagedList<CustomerViewModel> customersToDisplayPerPage = allCustomers.ToPagedList(pageNumber, IntegerConstants.ItemsPerPage);
                return this.View(customersToDisplayPerPage);
            }
            catch {
                return this.View(null);
            }
        }
    }
}
