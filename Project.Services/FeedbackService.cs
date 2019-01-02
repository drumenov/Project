using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project.Data;
using Project.Models.Entities;
using Project.Services.Contracts;


namespace Project.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<User> userManager;

        public FeedbackService(ApplicationDbContext dbContext, 
            UserManager<User> userManager) {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public FeedbackService(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public async Task CreateFeedbackAsync(Feedback feedback) {
            this.dbContext
                .Feedbacks
                .Add(feedback);
            if(await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
        }

        public Feedback GetByRepairTaskId(int id) {
            return this.dbContext
                .Feedbacks
                .FirstOrDefault(feedback => feedback.RepairTask.Id == id);
        }

        public async Task UpdateFeedbackAsync(Feedback feedback) {
            Feedback feedbackToUpdate = this.dbContext.Feedbacks.FirstOrDefault(x => feedback.RepairTask.Id == x.RepairTask.Id);
            if(feedbackToUpdate == null) {
                throw new ApplicationException();
            }
            feedbackToUpdate.Content = feedback.Content;
            if(await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
        }
    }
}
