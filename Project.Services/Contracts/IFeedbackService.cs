using Project.Models.Entities;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public interface IFeedbackService
    {
        Task CreateFeedback(Feedback feedback);
    }
}
