using Project.Models.Entities;
using Project.Models.InputModels.Customer;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public interface IRepairTaskService
    {
        Task<int> CreateRepairTaskAsync(RepairTaskInputModel repairTaskInputModel, User user);

        RepairTask GetById(int id);

        IQueryable<RepairTask> GetAllPending();

        IQueryable<RepairTask> GetAllWorkedByATechnician(string technicianId);

        IQueryable<RepairTask> GetAllWorkedOn();

        IQueryable<RepairTask> GetAllFinishedRepairTasks();
    }
}
