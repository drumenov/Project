using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Models.InputModels.Administration;

namespace Project.Areas.Administration.Controllers
{
    public class PartController : BaseController
    {
        [HttpGet]
        public IActionResult Order() => this.View();

        [HttpPost]
        public IActionResult Order(CreatePartOrderInputModel createPartOrderInputModel) {
            if (ModelState.IsValid) {

            }
            return this.View();
        }
    }
}
