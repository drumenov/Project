using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Common;
using Project.Models.Entities;
using Project.Models.ViewModels;
using Project.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Project.Areas.Administration.Components
{
    [ViewComponent(Name = StringConstants.ViewComponentTechniciansName)]
    public class TechnicianViewComponent : ViewComponent
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public TechnicianViewComponent(IUserService userService,
            IMapper mapper) {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? page) {
            try {
                int pageNumber = page ?? 1;
                string[] allTechnicianLevelsNames = new string[] {
                    StringConstants.NoviceTechnicianUserRole,
                    StringConstants.AverageTechnicianUserRole,
                    StringConstants.AdvancedTechnicianUserRole,
                    StringConstants.ExpertTechnicianUserRole
                };

                List<TechnicianViewModel> allTechnicians = new List<TechnicianViewModel>();
                foreach(string technicianLevel in allTechnicianLevelsNames) {
                    allTechnicians.AddRange(await this.userService.GetAllUsersWithAGivenRoleAsync(technicianLevel) != null
                    ? this.mapper.ProjectTo<TechnicianViewModel>(await this.userService.GetAllUsersWithAGivenRoleAsync(technicianLevel)).ToArray()
                    : new TechnicianViewModel[0]);
                }

                //TechnicianViewModel[] allTechnicians = this.mapper
                //    .ProjectTo<TechnicianViewModel>(await this.userService.GetAllUsersWithAGivenRoleAsync(StringConstants.TechnicianUserRole))
                //    .ToArray();
                int maximumNumberOfPages = allTechnicians.Count() / IntegerConstants.ItemsPerPage;
                if (allTechnicians.Count() > IntegerConstants.ItemsPerPage && allTechnicians.Count() % IntegerConstants.ItemsPerPage != 0) {
                    maximumNumberOfPages += 1;
                }
                if (pageNumber <= 0 || pageNumber > maximumNumberOfPages) { //This check for the feasibility of the page number. If it is "out of range" the default value of 1 is selected."
                    pageNumber = 1;
                }
                TempData[StringConstants.TempDataKeyHoldingNumberOfMaximumPagesForTechnicians] = maximumNumberOfPages;
                IPagedList<TechnicianViewModel> adminsToDisplayOnPage = allTechnicians.ToPagedList(pageNumber, IntegerConstants.ItemsPerPage);
                return this.View(adminsToDisplayOnPage);
            }
            catch {
                return this.View(null);
            }
        }
    }
}
