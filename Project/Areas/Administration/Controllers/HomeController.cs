using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common;

namespace Project.Areas.Administration.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index(int? page) {
            int currentPage = page ?? 1;
            TempData["page"] = currentPage;
            return this.View();
        }
    }
    
}
