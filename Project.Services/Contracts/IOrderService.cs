using Project.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public interface IOrderService
    {
        Order GetOrderById(int orderId);
    }
}
