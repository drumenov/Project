using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common;
using Project.Models.InputModels.Administration;

namespace Project.Areas.Administration.Controllers
{
    public class PartController : BaseController
    {
        [HttpGet]
        public IActionResult Order() => this.View();

        [HttpPost]
        public IActionResult Order(CreatePartOrderInputModel createPartOrderInputModel) {
            if (!ModelState.IsValid) {
                return this.View(createPartOrderInputModel);
            }
            if(createPartOrderInputModel.IsCarBodyPart == false
                && createPartOrderInputModel.IsChassisPart == false
                && createPartOrderInputModel.IsElectronicPart == false
                && createPartOrderInputModel.IsInteriorPart == false) {
                this.ModelState.AddModelError("", StringConstants.WrongOrder);
                return this.View(createPartOrderInputModel);
            }

            return this.View();
        }
    }
}
