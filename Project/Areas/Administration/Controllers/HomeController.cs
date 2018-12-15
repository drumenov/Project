using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;

namespace Project.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class HomeController : BaseController
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Index() {
            return this.View();
        }
    }
}
