using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Constants;
using Project.Controllers.Base;
using Project.Models.Entities;
using System;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class HomeController : BaseController
    {
        private readonly UserManager<User> userManager;

        public HomeController(UserManager<User> userManager) {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index() {
            if (this.User.Identity.IsAuthenticated == false) {
                return this.View(); //TODO: Make proper redirect when user is authenitcated   
            }
            else {
                User user = await this.userManager.FindByNameAsync(this.User.Identity.Name);
                if (await this.userManager.IsInRoleAsync(user, StringConstants.AdminUserRole)) {
                    return this.RedirectToAction(StringConstants.ActionNameIndex,
                                                    StringConstants.HomeControllerName,
                                                    new { area = StringConstants.AreaNameAdministration });
                } else if (await this.userManager.IsInRoleAsync(user, StringConstants.CustomerUserRole)
                          || await this.userManager.IsInRoleAsync(user, StringConstants.CorporateCustomerUserRole)) {
                            return this.RedirectToAction(StringConstants.ActionNameIndex,
                                                    StringConstants.HomeControllerName,
                                                    new { area = StringConstants.AreaNameCustomer });
                } else if (await this.userManager.IsInRoleAsync(user, StringConstants.NoviceTechnicianUserRole)
                          || await this.userManager.IsInRoleAsync(user, StringConstants.AverageTechnicianUserRole)
                          || await this.userManager.IsInRoleAsync(user, StringConstants.AdvancedTechnicianUserRole)
                          || await this.userManager.IsInRoleAsync(user, StringConstants.ExpertTechnicianUserRole)){
                    return this.RedirectToAction(StringConstants.ActionNameIndex,
                                                    StringConstants.HomeControllerName,
                                                    new { area = StringConstants.AreaNameTechnician });
                } else {
                    throw new ApplicationException();
                }
            }
        }

        public IActionResult Error() => this.View();

        [HttpGet]
        [Route("/[controller]/unauthorised-access")]
        public IActionResult UnauthorisedAccess() => this.View();
    }
}
