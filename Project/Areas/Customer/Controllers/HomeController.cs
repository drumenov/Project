using Microsoft.AspNetCore.Mvc;
using Project.Areas.Customer.Controllers.Base;

namespace Project.Areas.Customer.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index() {
            return this.View(); //TODO: Think about using View Components here.
        }
    }
}
