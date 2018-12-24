using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public interface IReceiptService
    {
        Task GenerateReceiptAsync(int taskId);
    }
}
