using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common.Constants;
using Project.Models.Entities;
using Project.Models.InputModels.Administration;
using Project.Services.Contracts;
using System.Threading.Tasks;

namespace Project.Areas.Administration.Controllers
{
    public class PartController : BaseController
    {
        private readonly IPartService partService;
        private readonly UserManager<User> userManager;

        public PartController(IPartService partService,
            UserManager<User> userManager) {
            this.partService = partService;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Order() => this.View();

        [HttpPost]
        public async Task<IActionResult> Order(CreatePartOrderInputModel createPartOrderInputModel) {
            if (ModelState.IsValid == false) {
                return this.View(createPartOrderInputModel);
            }
            if(createPartOrderInputModel.IsCarBodyPart == false
                && createPartOrderInputModel.IsChassisPart == false
                && createPartOrderInputModel.IsElectronicPart == false
                && createPartOrderInputModel.IsInteriorPart == false) {
                this.ModelState.AddModelError("", StringConstants.WrongOrder);
                return this.View(createPartOrderInputModel);
            }
            User admin = await this.userManager.FindByNameAsync(this.User.Identity.Name);
            int orderId = await this.partService.OrderPartsAsync(createPartOrderInputModel, admin);
            return this.RedirectToAction(StringConstants.ActionNameOrderDetails, StringConstants.OrderControllerName, new { id = orderId });
        }
    }
}
