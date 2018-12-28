using Project.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public interface IReceiptService
    {
        Task GenerateReceiptAsync(ICollection<User> techniciansHavingWrokedOnRepairTask, string customerId, RepairTask repairTask);

        decimal GetTotalRevenuePerCustomer(string customerName);
    }
}
