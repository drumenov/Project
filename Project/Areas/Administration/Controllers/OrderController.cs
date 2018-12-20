using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common;
using Project.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Areas.Administration.Controllers
{
    public class OrderController : BaseController
    {
        [HttpGet]
        [Route("/administration/[controller]/order-details/{id}")]
        public IActionResult OrderDetails(int id) {
            return this.View(id);
        }
    }
}
