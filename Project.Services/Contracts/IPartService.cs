using Project.Models.Entities;
using Project.Models.InputModels.Administration;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public interface IPartService
    {
        Task<int> OrderPartsAsync(CreatePartOrderInputModel createPartOrderInputModel, User admin);
    }
}
