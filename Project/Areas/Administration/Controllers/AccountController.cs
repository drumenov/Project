using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common;
using Project.Models.Entities;
using Project.Models.InputModels.Administration;
using Project.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace Project.Areas.Administration.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public AccountController(
            IUserService userService,
            IMapper mapper) {
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("/administration/[controller]/create-admin")]
        public IActionResult CreateAdmin() => this.View(StringConstants.CreateAdministratorViewName);

        [HttpPost]
        [Route("/administration/[controller]/create-admin")]
        public async Task<IActionResult> CreateAdmin(CreateAdministratorInputModel createAdministratorInputModel) {
            if (!ModelState.IsValid) {
                return this.View(StringConstants.CreateAdministratorViewName, createAdministratorInputModel);
            }
            User newAdminUser = this.mapper.Map<User>(createAdministratorInputModel);
            bool success = await this.userService.RegisterUserAsync(newAdminUser, createAdministratorInputModel.Password);
            if (!success) {
                throw new ApplicationException();
            }
            bool succeded = await this.userService.AddUserToRoleAsync(newAdminUser, StringConstants.AdminUserRole);
            if (!succeded) {
                throw new ApplicationException();
            }
            return this.RedirectToAction(StringConstants.ActionNameIndex, StringConstants.HomeControllerName, new { area = StringConstants.AreaNameAdministration });
        }

        [HttpGet]
        [Route("/administration/[controller]/create-customer")]
        public IActionResult CreateCustomer() => this.View(StringConstants.CreateCustomerViewName);

        [HttpPost]
        [Route("/administration/[controller]/create-customer")]
        public IActionResult CreateCustomer(CreateCustomerInputModel createCustomerInputModel) {
            if (!ModelState.IsValid) {
                return this.View(StringConstants.CreateCustomerViewName, createCustomerInputModel);
            }
            return null;
        }

        [HttpGet]
        [Route("/administration/[controller]/create-technician")]
        public IActionResult CreateTechnician() => this.View(StringConstants.CreateTechnicianViewName);

        [HttpPost]
        [Route("/administration/[controller]/create-technician")]
        public IActionResult CreateTechnician(CreateTechnicianInputModel createTechnicianInputModel) {
            if (!ModelState.IsValid) {
                return this.View(StringConstants.CreateTechnicianViewName, createTechnicianInputModel);
            }
            return null; //TODO: Proper handling of request required
        }
    }
}
