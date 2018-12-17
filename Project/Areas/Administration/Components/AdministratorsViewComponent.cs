﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Common;
using Project.Models.ViewModels;
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
                AdministratorViewModel[] allAdmins = this.mapper.ProjectTo<AdministratorViewModel>(await this.userService.GetAllUsersWithAGivenRoleAsync(StringConstants.AdminUserRole)).ToArray();
                int maximumNumberOfPages = allAdmins.Count() / IntegerConstants.ItemsPerPage;
                if (allAdmins.Count() > IntegerConstants.ItemsPerPage && allAdmins.Count() % IntegerConstants.ItemsPerPage != 0) {
                    maximumNumberOfPages += 1;
                }
                if (pageNumber <= 0 || pageNumber > maximumNumberOfPages) { //This check for the feasibility of the page number. If it is "out of range" the default value of 1 is selected."
                    pageNumber = 1;
                }
                TempData[StringConstants.TempDataKeyHoldingNumberOfMaximumPagesForAdministrators] = maximumNumberOfPages;
                IPagedList<AdministratorViewModel> adminsToDisplayOnPage = allAdmins.ToPagedList(pageNumber, IntegerConstants.ItemsPerPage);
                return this.View(adminsToDisplayOnPage);
            }
            catch {
                return this.View(null);
            }
        }
    }
}