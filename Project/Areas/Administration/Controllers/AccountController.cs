using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common;
using Project.Models.InputModels.Administration;

namespace Project.Areas.Administration.Controllers
{
    [Area(StringConstants.AreaNameAdministration)]
    [Authorize(Roles = StringConstants.AdminUserRole)]
    public class AccountController : BaseController
    {

        [HttpGet]
        [Route("/administration/[controller]/create-admin")]
        public IActionResult CreateAdmin() => this.View("create-admin");

        [HttpPost]
        [Route("/administration/[controller]/create-admin")]
        public IActionResult CreateAdmin(CreateAdministratorInputModel createAdministratorInputModel) {
            if (!ModelState.IsValid) {
                return this.View("create-admin", createAdministratorInputModel);
            }
            return null; //TODO: Proper handling of request required
        }

        [HttpGet]
        [Route("/administration/[controller]/create-customer")]
        public IActionResult CreateCustomer() => this.View("create-customer");

        [HttpPost]
        [Route("/administration/[controller]/create-customer")]
        public IActionResult CreateCustomer(CreateCustomerInputModel createCustomerInputModel) {
            if (!ModelState.IsValid) {
                return this.View("create-customer", createCustomerInputModel);
            }
            return null;
        }

        [HttpGet]
        [Route("/administration/[controller]/create-technician")]
        public IActionResult CreateTechnician() => this.View("create-technician");

        [HttpPost]
        [Route("/administration/[controller]/create-technician")]
        public IActionResult CreateTechnician(CreateTechnicianInputModel createTechnicianInputModel) {
            if (!ModelState.IsValid) {
                return this.View("create-technician", createTechnicianInputModel);
            }
            return null; //TODO: Proper handling of request required
        }
    }
}
