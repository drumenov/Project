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
    [ViewComponent(Name = StringConstants.ViewComponentAdministratorsName)]
    public class AdministratorsViewComponent : ViewComponent
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public AdministratorsViewComponent(IUserService userService,
            IMapper mapper) {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? page) {
            try {
                int pageNumber = page ?? 1;
                AdministratorViewModel[] allAdmins = this.mapper
                    .ProjectTo<AdministratorViewModel>(await this.userService.GetAllUsersWithAGivenRoleAsync(StringConstants.AdminUserRole))
                    .ToArray();
                IPagedList<AdministratorViewModel> adminsToDisplayOnPage = allAdmins.ToPagedList(pageNumber, IntegerConstants.ItemsPerPage);
                return this.View(adminsToDisplayOnPage);
            }
            catch {
                return this.View(null);
            }
        }
    }
}
