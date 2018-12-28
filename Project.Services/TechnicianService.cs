using Microsoft.AspNetCore.Identity;
using Project.Common.Constants;
using Project.Data;
using Project.Models.Entities;
using Project.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Services
{
    public class TechnicianService : ITechnicianService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly IRepairTaskService repairTaskService;

        public TechnicianService(ApplicationDbContext dbContext,
            UserManager<User> userManager,
            IRepairTaskService repairTaskService) {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.repairTaskService = repairTaskService;
        }

        public async Task AddTechniciansToRepairTaskAsync(ICollection<string> availableTechniciansName, int taskId) {
            foreach (string technicianName in availableTechniciansName) {
                User user = await this.userManager.FindByNameAsync(technicianName);
                this.dbContext
                    .UsersRepairsTasks
                    .Add(new UserRepairTask {
                        RepairTaskId = taskId,
                        UserId = user.Id,
                        IsFinished = false,
                    });
            }
            RepairTask repairTask = this.repairTaskService.GetById(taskId);
            repairTask.Status = Models.Enums.Status.WorkedOn;
            if (await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
        }

        public async Task<User[]> GetAllAvailableTechnicians() {
            string[] allTechnicianRoles = new[] {
                StringConstants.NoviceTechnicianUserRole,
                StringConstants.AverageTechnicianUserRole,
                StringConstants.AdvancedTechnicianUserRole,
                StringConstants.ExpertTechnicianUserRole
            };
            List<User> allTechnicians = new List<User>();
            foreach (var technicianLevel in allTechnicianRoles) {
                allTechnicians.AddRange(await this.userManager.GetUsersInRoleAsync(technicianLevel));
            }
            User[] availableTechnicians = allTechnicians
                .Where(x => x.RepairTasks.Count < IntegerConstants.ThresholdDefiningAvailableTechnician)
                .ToArray();
            return availableTechnicians;
        }

        public IQueryable<RepairTask> GetAllFinishedRepairTaskPerTechnician(string technicianName) {
            string technicianId = this.userManager.FindByNameAsync(technicianName).GetAwaiter().GetResult().Id;
            return this.dbContext
                .UsersRepairsTasks
                .Where(userRepairTask => userRepairTask.UserId == technicianId && userRepairTask.IsFinished)
                .Select(finishedRepairTasksByTechnician => finishedRepairTasksByTechnician.RepairTask);
        }

        public async Task<IEnumerable<string>> GetAllNamesOfTechniciansNotWorkingOnAGivenTask(int taskId) {

            List<User> allUsersWithoutAnyRepairTasks = this.dbContext
                .Users
                .Where(user => user.RepairTasks.Count == 0)
                .ToList();

            foreach (User user in allUsersWithoutAnyRepairTasks.ToList()) {
                if (await this.userManager.IsInRoleAsync(user, StringConstants.AdminUserRole)
                    || await this.userManager.IsInRoleAsync(user, StringConstants.CustomerUserRole)
                    || await this.userManager.IsInRoleAsync(user, StringConstants.CorporateCustomerUserRole)) {
                    allUsersWithoutAnyRepairTasks.Remove(user);
                }
            }

            List<string> namesOfTechniciansWithoutAnyRepairTasks = allUsersWithoutAnyRepairTasks.Select(user => user.UserName).ToList();

            var techniciansWorkingOnSomeTasks = this.dbContext
                 .Users
                 .Where(user => user.RepairTasks.Count < IntegerConstants.ThresholdDefiningAvailableTechnician)
                 .SelectMany(user => user.RepairTasks)
                 .Where(filteredUsers => filteredUsers.RepairTaskId != taskId)
                 .Select(filteredRepairTasks => filteredRepairTasks.Expert.UserName);
            var techniciansThatCanBeAssigned = namesOfTechniciansWithoutAnyRepairTasks.Union(techniciansWorkingOnSomeTasks);
            return techniciansThatCanBeAssigned;
        }

        public IQueryable<string> GetAllNamesOfTechniciansWorkingOnAGivenTask(int taskId) {
            return this.dbContext
                .RepairTasks
                .Where(repairTask => repairTask.Id == taskId)
                .SelectMany(filteredRepairTasks => filteredRepairTasks.Technicians)
                .Where(techniciansNotHavingFinishedTheGivenTask => techniciansNotHavingFinishedTheGivenTask.IsFinished == false)
                .Select(technician => technician.Expert.UserName);
        }
    }
}
