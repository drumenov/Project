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

        IQueryable<RepairTask> GetAllFinished();

        Task<IQueryable<RepairTask>> GetPendingPerCustomerAsync(string customerName);

        Task<IQueryable<RepairTask>> GetWorkedOnPerCustomerAsync(string customerName);

        Task<IQueryable<RepairTask>> GetFinishedPerCustomerAsync(string customerName);

        Task TechnicianCompletesARepairTaskAsync(int repairTaskId, string technicianName);

        IQueryable<User> GetTechniciansHavingWorkedOnARepairTask(int repairTaskId);

        Task RemoveTechnicianFromRepairTaskAsync(string nameOfTechnicianToRemove, int id);

        Task AddTechnicianToRepairTaskAsync(string nameOfTechnicianToAdd, int id);

        IQueryable<RepairTask> GetAllPerCustomer(string customerName);

        Task UpdateRepairTaskAsync(RepairTaskEditInputModel repairTaskEditInputModel);
    }
}
