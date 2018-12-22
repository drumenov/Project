using Project.Models.Entities;
using Project.Models.ViewModels.Administration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public interface ITechnicianService
    {
        Task<User[]> GetAllAvailableTechnicians();

        Task AddTechniciansToRepairTaskAsync(ICollection<string> availableTechniciansName, int taskId);
    }
}
