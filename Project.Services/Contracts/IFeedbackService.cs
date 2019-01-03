using Project.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public interface IFeedbackService
    {
        Task CreateFeedbackAsync(Feedback feedback);

        Feedback GetByRepairTaskId(int id);

        Task UpdateFeedbackAsync(Feedback feedback);

        Task EditFeedbackAsync(Feedback feedback);

        IQueryable<Feedback> GetAllPerCustomer(string username);
    }
}
