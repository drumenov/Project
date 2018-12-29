using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Administration.Controllers.Base;
using Project.Common.Constants;
using Project.Models.Entities;
using Project.Models.ViewModels.Administration;
using Project.Services.Contracts;
using System.Threading.Tasks;

namespace Project.Areas.Administration.Controllers
{
    public class TechnicianController : BaseController
    {
        private readonly UserManager<User> userManager;
        private readonly ITechnicianService technicianService;
        private readonly IMapper mapper;

        public TechnicianController(UserManager<User> userManager,
            ITechnicianService technicianService,
            IMapper mapper) {
            this.userManager = userManager;
            this.technicianService = technicianService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("/administration/[controller]/[action]/{technicianName}")]
        public async Task<IActionResult> Promote(string technicianName) {
            await this.technicianService.PromoteTechnicianAsync(technicianName);
            return this.RedirectToAction(StringConstants.ActionNameTechnicianDetails, new { technicianName});
        }

        [HttpPost]
        [Route("/administration/[controller]/[action]/{technicianName}")]
        public async Task<IActionResult> Demote(string technicianName) {
            await this.technicianService.DemoteTechnicianAsync(technicianName);
            return this.RedirectToAction(StringConstants.ActionNameTechnicianDetails, new { technicianName });
        }

        [HttpGet]
        [Route("/administration/[controller]/technician-details/{technicianName}")]
        public async Task<IActionResult> Details(string technicianName) {
            TechnicianDetailsViewModel technicianDetailsViewModel = this.mapper
                                                                        .Map<TechnicianDetailsViewModel>(await this.userManager.FindByNameAsync(technicianName));
            return this.View(technicianDetailsViewModel);
        }
    }
}
