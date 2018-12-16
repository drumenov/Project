using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Controllers.Base;

namespace Project.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index() {
            return this.View(); //TODO: Make proper redirect when user is authenitcated   
        }

        public IActionResult Error() => this.View();
    }
}
