using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Models.Entities;
using Project.Models.ViewModels.Administration;
using Project.Services.Contracts;

namespace Project.Areas.Administration.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public OrderController(IOrderService orderService,
            IMapper mapper,
            UserManager<User> userManager) {
            this.orderService = orderService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("/administration/[controller]/order-details/{id}")]
        public IActionResult OrderDetails(int id) {
            Order order = this.orderService.GetOrderById(id);
            OrderViewModel orderViewModel = new OrderViewModel {
                Id = id,
                OrderedParts = order.OrderedParts,
                Username = order.User.UserName
            }; //TODO: Try to do this useing automapper.
            return this.View(orderViewModel);
        }
    }
}
