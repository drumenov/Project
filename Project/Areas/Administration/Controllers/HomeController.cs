using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common;

namespace Project.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class HomeController : BaseController
    {
        [Authorize(Roles = StringConstants.AdminUserRole)]
        public IActionResult Index() => this.View();
    }
}
