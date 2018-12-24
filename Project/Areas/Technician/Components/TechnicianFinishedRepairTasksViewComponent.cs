using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Constants;
using Project.Models.ViewModels.Technician;
using Project.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Project.Areas.Technician.Components
{
    [ViewComponent(Name = StringConstants.ViewComponentFinishedRepairTasksByTechnician)]
    public class TechnicianFinishedRepairTasksViewComponent : ViewComponent
    {
        private readonly ITechnicianService technicianService;
        private readonly IMapper mapper;

        public TechnicianFinishedRepairTasksViewComponent(ITechnicianService technicianService,
            IMapper mapper) {
            this.technicianService = technicianService;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page) {
            RepairTaskViewModel[] allFinishedRepairTasksByTechnician = this.mapper
                .ProjectTo<RepairTaskViewModel>(this.technicianService
                                                        .GetAllFinishedRepairTaskPerTechnician(this.User.Identity.Name))
                .ToArray();
            IPagedList<RepairTaskViewModel> finishedRepairTaskByTechnicianToDisplay = allFinishedRepairTasksByTechnician
                                                                                        .ToPagedList(page, IntegerConstants.ItemsPerPage);
            return this.View(finishedRepairTaskByTechnicianToDisplay);
        }
    }
}
