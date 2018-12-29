using Project.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public interface ITechnicianService
    {
        Task<User[]> GetAllAvailableTechnicians();

        Task AddTechniciansToRepairTaskAsync(ICollection<string> availableTechniciansName, int taskId);

        IQueryable<RepairTask> GetAllFinishedRepairTaskPerTechnician(string technicianName);

        Task<IEnumerable<string>> GetAllNamesOfTechniciansNotWorkingOnAGivenTask(int taskId);

        IQueryable<string> GetAllNamesOfTechniciansWorkingOnAGivenTask(int taskId);

        Task PromoteTechnicianAsync(string technicianName);

        Task DemoteTechnicianAsync(string technicianName);
    }
}
