using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common.Constants;
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
        public IActionResult CreateAdmin() => this.View();

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
            success = await this.userService.AddUserToRoleAsync(newAdminUser, StringConstants.AdminUserRole);
            if (!success) {
                throw new ApplicationException();
            }
            return this.RedirectToAction(StringConstants.ActionNameIndex, StringConstants.HomeControllerName, new { area = StringConstants.AreaNameAdministration });
        }

        [HttpGet]
        [Route("/administration/[controller]/create-customer")]
        public IActionResult CreateCustomer() => this.View();

        [HttpPost]
        [Route("/administration/[controller]/create-customer")]
        public async Task<IActionResult> CreateCustomer(CreateCustomerInputModel createCustomerInputModel) {
            if (!ModelState.IsValid) {
                return this.View(StringConstants.CreateCustomerViewName, createCustomerInputModel);
            }
            User newCustomer = this.mapper.Map<User>(createCustomerInputModel);
            bool success = await this.userService.RegisterUserAsync(newCustomer, createCustomerInputModel.Password);
            if (!success) {
                throw new ApplicationException();
            }
            if (createCustomerInputModel.IsCorporateCustomer) {
                success = await this.userService.AddUserToRoleAsync(newCustomer, StringConstants.CorporateCustomerUserRole);
            } else {
                success = await this.userService.AddUserToRoleAsync(newCustomer, StringConstants.CustomerUserRole);
            }
            if (!success) {
                throw new ApplicationException();
            }
            return this.RedirectToAction(StringConstants.ActionNameIndex, StringConstants.HomeControllerName, new { area = StringConstants.AreaNameAdministration});
        }

        [HttpGet]
        [Route("/administration/[controller]/create-technician")]
        public IActionResult CreateTechnician() => this.View();

        [HttpPost]
        [Route("/administration/[controller]/create-technician")]
        public async Task<IActionResult> CreateTechnician(CreateTechnicianInputModel createTechnicianInputModel) {
            if (!ModelState.IsValid) {
                return this.View(StringConstants.CreateTechnicianViewName, createTechnicianInputModel);
            }
            User newTechnician = this.mapper.Map<User>(createTechnicianInputModel);
            bool success = await this.userService.RegisterUserAsync(newTechnician, createTechnicianInputModel.Password);
            if(!success) {
                throw new ApplicationException();
            }
            success = await this.userService.AddUserToRoleAsync(newTechnician, String.Concat(createTechnicianInputModel.Level.ToString(), StringConstants.TechnicianUserRole));
            
            if (!success) {
                throw new ApplicationException();
            }
            return this.RedirectToAction(StringConstants.ActionNameIndex, StringConstants.HomeControllerName, new { area = StringConstants.AreaNameAdministration });
        }
    }
}
