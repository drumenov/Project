using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Customer.Controllers.Base;
using Project.Common.Constants;
using Project.Models.Entities;
using Project.Models.InputModels.Customer;
using Project.Services.Contracts;
using System.Threading.Tasks;

namespace Project.Areas.Customer.Controllers
{
    public class FeedbackController : BaseController
    {
        private readonly IRepairTaskService repairTaskService;
        private readonly IFeedbackService feedbackService;
        private readonly IMapper mapper;

        public FeedbackController(IRepairTaskService repairTaskService,
            IFeedbackService feedbackService,
            IMapper mapper) {
            this.repairTaskService = repairTaskService;
            this.feedbackService = feedbackService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("/customer/[controller]/repair-task-feedback/{id}")]
        public IActionResult GiveFeedBack(int id) {
            RepairTask repairTask = this.repairTaskService.GetById(id);
            if (repairTask.User.UserName != this.User.Identity.Name) {
                return this.Unauthorized();
            }
            FeedbackInputModel feedbackInputModel = new FeedbackInputModel {
                RepairTaskId = id
            };
            return this.View(feedbackInputModel);
        }

        [HttpPost]
        [Route("/customer/[controller]/repair-task-feedback/{id}")]
        public async Task<IActionResult> GiveFeedbackAsync(FeedbackInputModel feedbackInputModel) {
            if (ModelState.IsValid == false) {
                return this.View(feedbackInputModel);
            }
            Feedback feedback = this.mapper.Map<Feedback>(feedbackInputModel);
            await this.feedbackService.CreateFeedbackAsync(feedback);
            return this.RedirectToAction(StringConstants.ActionNameIndex, StringConstants.HomeControllerName);
        }

        [HttpGet]
        [Route("/customer/[controller]/repair-task-edit-feedback/{id}")]
        public IActionResult EditFeedback(int id) {
            FeedbackInputModel feedbackInputModel = this.mapper.Map<FeedbackInputModel>(this.feedbackService.GetByRepairTaskId(id));
            return this.View(feedbackInputModel);
        }

        [HttpPost]
        [Route("/customer/[controller]/repair-task-edit-feedback/{id}")]
        public async Task<IActionResult> EditFeedbackAsync(FeedbackInputModel feedbackInputModel) {
            Feedback feedback = this.feedbackService.GetByRepairTaskId(feedbackInputModel.RepairTaskId);
            if (feedback.Content.Equals(feedbackInputModel.Content) == false) {
                ModelState.AddModelError("", StringConstants.NoChangesWhenEditingFeedbackError);
                return this.View(feedbackInputModel);
            }
            feedback = this.mapper.Map<Feedback>(feedbackInputModel);
            await this.feedbackService.UpdateFeedbackAsync(feedback);
            return this.RedirectToAction(StringConstants.ActionNameIndex, StringConstants.HomeControllerName);
        }
    }
}
