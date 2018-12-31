using System.Threading.Tasks;
using Project.Data;
using Project.Models.Entities;
using Project.Models.InputModels.Customer;
using Project.Services.Contracts;


namespace Project.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ApplicationDbContext dbContext;

        public FeedbackService(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public Task CreateFeedback(Feedback feedback) {
            throw new System.NotImplementedException();
        }
    }
}
