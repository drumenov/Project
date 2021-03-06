﻿using Microsoft.AspNetCore.Mvc;
using Project.Controllers.Base;
using Project.Models.InputModels;
using Project.Services.Contracts;
using System.Threading.Tasks;
using System;
using Project.Common.Constants;

namespace Project.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService) {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Login(string userRole) {
            TempData[StringConstants.TempDataKeyHoldingUserRole] = userRole;
            return this.View(); //TODO: Make proper redirect when user is logged in
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserInputModel loginUserInputModel) {
            if (!ModelState.IsValid) {
                return this.View(loginUserInputModel);
            }
            //await this.userService.LogoutUserAsync();
            bool success = await this.userService.LoginUserAsync(loginUserInputModel.Username, loginUserInputModel.Password);
            if (!success) {
                ModelState.AddModelError(string.Empty, StringConstants.WrongUsernameOrPasswordErrorMessage);
                return this.View(loginUserInputModel);
            }

            //If we have reached this point, it is more or less safe to assume that the user exists, hence we next check to which area we are to redirect him - Administration, Customer or Technician

            if (TempData[StringConstants.TempDataKeyHoldingUserRole].ToString().Equals(StringConstants.AdminUserRole, StringComparison.OrdinalIgnoreCase)) { //Checks whether the user is trying to reach the Administration's area
                    return this.RedirectToAction(
                        StringConstants.ActionNameIndex, //Action's name
                        StringConstants.HomeControllerName, //Controller's name
                        new { area = StringConstants.AreaNameAdministration }); //Area's name
            } else if(TempData[StringConstants.TempDataKeyHoldingUserRole].ToString().Equals(StringConstants.CustomerUserRole, StringComparison.OrdinalIgnoreCase)) { //Checks whether the user is trying to reach the Customer's area
                return this.RedirectToAction(
                    StringConstants.ActionNameIndex, //Action's name
                    StringConstants.HomeControllerName, //Controller's name
                    new { area = StringConstants.AreaNameCustomer }); // Area's name
            } else if (TempData[StringConstants.TempDataKeyHoldingUserRole].ToString().Equals(StringConstants.TechnicianUserRole, StringComparison.OrdinalIgnoreCase)) { //Checks whether the user is trying to reach the Technician's area
                return this.RedirectToAction(
                    StringConstants.ActionNameIndex, //Action's name
                    StringConstants.HomeControllerName, //Controller's name
                    new { area = StringConstants.AreaNameTechnician }); //Area's name
            }
            throw new NotImplementedException(); //TODO: If we are to reach this point, some unauthorised routing is used, hence an error is thrown and the user should be redirected to a generic error page
        }

        public IActionResult Logout() {
            this.userService.LogoutUserAsync();
            return this.RedirectToAction(StringConstants.ActionNameIndex, StringConstants.HomeControllerName);
        }
    }
}
