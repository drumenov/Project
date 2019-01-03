using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Customer.Controllers.Base;
using Project.Common.Constants;
using Project.Models.Entities;
using Project.Models.InputModels.Customer;
using Project.Models.ViewModels.Customer;
using Project.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

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
            feedback.RepairTask = this.repairTaskService.GetById(feedbackInputModel.RepairTaskId);
            await this.feedbackService.CreateFeedbackAsync(feedback);
            return this.RedirectToAction(StringConstants.ActionNameAllFeedbacks);
        }

        [HttpGet]
        [Route("/customer/[controller]/repair-task-edit-feedback/{id}")]
        public IActionResult EditFeedback(int id) {
            FeedbackInputModel feedbackInputModel = this.mapper.Map<FeedbackInputModel>(this.feedbackService.GetByRepairTaskId(id));
            return this.View(StringConstants.ViewForGivingAndEditingFeedback, feedbackInputModel);
        }

        [HttpPost]
        [Route("/customer/[controller]/repair-task-edit-feedback/{id}")]
        public async Task<IActionResult> EditReprTaskFeedbackAsync(FeedbackInputModel feedbackInputModel) {
            Feedback feedback = this.mapper.Map<Feedback>(feedbackInputModel);
            feedback.RepairTask = this.repairTaskService.GetById(feedbackInputModel.RepairTaskId);
            await this.feedbackService.EditFeedbackAsync(feedback);
            return this.RedirectToAction(StringConstants.ActionNameAllFeedbacks);
        }

        [HttpGet]
        [Route("/customer/[controller]/all-feedbacks")]
        public IActionResult AllFeedbacks(int? page) {
            int currentPage = page ?? 1;
            FeedbackViewModel[] feedbackViewModels = this.mapper
                                                            .ProjectTo<FeedbackViewModel>(this.feedbackService.GetAllPerCustomer(this.User.Identity.Name))
                                                            .ToArray();
            IPagedList feedbacksToDisplay = feedbackViewModels.ToPagedList(currentPage, IntegerConstants.ItemsPerPageInViews);
            return this.View(feedbacksToDisplay);
        }
    }
}
