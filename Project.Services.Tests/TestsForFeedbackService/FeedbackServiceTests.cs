using Project.Common.Constants;
using Project.Models.Entities;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Project.Services.Tests.TestsForFeedbackService
{
    public class FeedbackServiceTests : Base
    {
        [Fact]
        public async Task CanCreateFeedback() {
            Assert.Empty(this.dbContext.Feedbacks);
            Feedback feedback = new Feedback();
            await this.FeedbackService.CreateFeedbackAsync(feedback);
            Assert.NotEmpty(this.dbContext.Feedbacks);
        }

        [Fact]
        public async Task ReturnsTheAppropriateFeedback() {
            Feedback feedback = new Feedback {
                RepairTask = new RepairTask { Id = 1 }
            };
            await this.FeedbackService.CreateFeedbackAsync(feedback);
            Assert.Same(feedback, this.FeedbackService.GetByRepairTaskId(feedback.RepairTask.Id));
        }

        [Fact]
        public async Task DoesNotReturnTheGivenFeedbackSinceItIsForAnotherRepairTask() {
            Feedback[] feedbacks = {
                new Feedback{RepairTask = new RepairTask{Id = 1}},
                new Feedback{RepairTask = new RepairTask{Id = 2}}
            };
            foreach (Feedback feedback in feedbacks) {
                await this.FeedbackService.CreateFeedbackAsync(feedback);
            }
            Assert.NotSame(feedbacks[0], this.FeedbackService.GetByRepairTaskId(feedbacks[1].RepairTask.Id));
        }

        [Fact]
        public async Task CanUpdateFeedback() {
            Feedback feedback = new Feedback {
                Content = "wow",
                RepairTask = new RepairTask { Id = 1 }
            };
            await this.FeedbackService.CreateFeedbackAsync(feedback);
            Assert.Equal(feedback.Content, this.FeedbackService.GetByRepairTaskId(feedback.RepairTask.Id).Content);
            Feedback updatedFeedback = new Feedback {
                Content = "Double wow",
                RepairTask = new RepairTask { Id = 1 }
            };
            await this.FeedbackService.UpdateFeedbackAsync(updatedFeedback);
            Assert.Equal(updatedFeedback.Content, this.FeedbackService.GetByRepairTaskId(feedback.RepairTask.Id).Content);
        }

        [Fact]
        public async Task CanEditFeedback() {
            Feedback feedback = new Feedback {
                Content = "wow",
                RepairTask = new RepairTask { Id = 1 }
            };
            await this.FeedbackService.CreateFeedbackAsync(feedback);
            Assert.Equal(feedback.Content, this.FeedbackService.GetByRepairTaskId(feedback.RepairTask.Id).Content);
            Feedback editedFeedback = new Feedback {
                Content = "double wow",
                RepairTask = new RepairTask { Id = 1 }
            };
            await this.FeedbackService.EditFeedbackAsync(editedFeedback);
            Assert.Equal(editedFeedback.Content, this.FeedbackService.GetByRepairTaskId(feedback.RepairTask.Id).Content);
        }

        [Fact]
        public async Task AllFeedbacksForACustomerAreReturned() {
            User customer = new User { UserName = "test" };
            await this.UserManager.CreateAsync(customer);
            await this.RoleManager.CreateAsync(new AppRole(StringConstants.CustomerUserRole));
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            Feedback[] feedbacks = new Feedback[3];
            for (int i = 0; i < feedbacks.Length; i++) {
                feedbacks[i] = new Feedback {
                    Customer = customer
                };
                await this.FeedbackService.CreateFeedbackAsync(feedbacks[i]);
            }
            Assert.Equal(feedbacks.Length, this.FeedbackService.GetAllPerCustomer(customer.UserName).ToArray().Length);
        }

        [Fact]
        public async Task OnlyFeedbacksForAGivenCustomerAreReturned() {
            await this.RoleManager.CreateAsync(new AppRole(StringConstants.CustomerUserRole));
            User[] customers = {
                new User {UserName = "test" },
                new User{UserName = "test2"}
                };
            foreach (User customer in customers) {
                await this.UserManager.CreateAsync(customer);
                await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            }
            Feedback[] feedbacks = new Feedback[3];
            for (int i = 0; i < feedbacks.Length; i++) {
                feedbacks[i] = new Feedback {
                    Customer = customers[0]
                };
            }
            feedbacks[0].Customer = customers[1];
            this.dbContext.Feedbacks.AddRange(feedbacks);
            this.dbContext.SaveChanges();
            Assert.Equal(feedbacks.Length - 1, this.FeedbackService.GetAllPerCustomer(customers[0].UserName).ToArray().Length);
        }
    }
}
