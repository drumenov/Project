using Project.Models.Entities;
using Project.Models.Enums;
using Project.Models.InputModels.Administration;
using Project.Models.ViewModels.Administration;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public interface IPartService
    {
        Task<int> OrderPartsAsync(CreatePartOrderInputModel createPartOrderInputModel, User admin);

        bool PartTypeExists(PartType partType);

        void AllPartsForRepairTaskAreAvailable(RepairTaskSimpleInfoViewModel repairTaskSimpleInfoViewModel);
    }
}
