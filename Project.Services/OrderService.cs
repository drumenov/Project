using Project.Data;
using Project.Models.Entities;
using Project.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext dbContext;

        public OrderService(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public Order GetOrderById(int orderId) {
            Order order = this.dbContext
                .Orders
                .FirstOrDefault(o => o.Id == orderId);
            return order;
        }

        public IQueryable<Order> GetAll() {
            return this.dbContext.Orders;
        }
    }
}
