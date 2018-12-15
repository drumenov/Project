using Microsoft.AspNetCore.Mvc;
using Project.Controllers.Base;

namespace Project.Controllers
{
    public class HomeController : BaseController
    {
        //[Route("/home/index")]
        public IActionResult Index() {
            return this.View(); //TODO: Make proper redirect when user is authenitcated   
        }
    }
}
