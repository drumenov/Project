using Project.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public interface IReceiptService
    {
        Task GenerateReceiptAsync(ICollection<User> techniciansHavingWrokedOnRepairTask, string customerId, RepairTask repairTask);

        decimal? GetTotalRevenuePerCustomer(string customerName);

        IQueryable<Receipt> GetAll();

        IQueryable<Receipt> GetById(int id);
    }
}
