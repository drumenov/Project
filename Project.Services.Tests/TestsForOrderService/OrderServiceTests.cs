using Project.Models.Entities;
using System.Linq;
using Xunit;

namespace Project.Services.Tests.TestsForOrderService
{
    public class OrderServiceTests : Base
    {
        [Fact]
        public void ReturnsTheCorrectOrder() {
            Order order = new Order {
                Id = 1
            };
            this.dbContext.Orders.Add(order);
            this.dbContext.SaveChanges();
            Assert.Same(order, this.OrderService.GetOrderById(order.Id));
        }

        [Fact]
        public void ReturnsTheCorrectOrderEvenWhenThereAreMultipleUniqueOrdersInDb() {
            Order[] orders = new Order[3];
            for(int i = 0; i < orders.Length; i++) {
                orders[i] = new Order { Id = i + 1 };
            }
            this.dbContext.Orders.AddRange(orders);
            this.dbContext.SaveChanges();
            Assert.Same(orders[0], this.OrderService.GetOrderById(orders[0].Id));
        }

        [Fact]
        public void ReturnsAllOrders() {
            Order[] orders = new Order[3];
            for (int i = 0; i < orders.Length; i++) {
                orders[i] = new Order { Id = i + 1 };
            }
            this.dbContext.Orders.AddRange(orders);
            this.dbContext.SaveChanges();
            Assert.Equal(orders.Length, this.OrderService.GetAll().ToArray().Length);
        }
    }
}
