using Microsoft.AspNetCore.Identity;
using Project.Data;
using Project.Models.Entities;
using Project.Models.InputModels.Customer;
using Project.Services.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Services
{
    public class RepairTaskService : IRepairTaskService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<User> userManager;

        public RepairTaskService(ApplicationDbContext dbContext,
            UserManager<User> userManager) {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<int> CreateRepairTaskAsync(RepairTaskInputModel repairTaskInputModel, User user) {
            RepairTask repairTask = new RepairTask {
                Status = Models.Enums.Status.Pending,
                User = user
            };
            if (repairTaskInputModel.IsCarBodyPart) {
                Part part = new Part {
                    Quantity = repairTaskInputModel.CarBodyPartAmount,
                    Type = Models.Enums.PartType.CarBody
                };
                repairTask.PartsRequired.Add(part);
            }
            if (repairTaskInputModel.IsChassisPart) {
                Part part = new Part {
                    Type = Models.Enums.PartType.Chassis,
                    Quantity = repairTaskInputModel.ChassisPartAmount
                };
                repairTask.PartsRequired.Add(part);
            }
            if (repairTaskInputModel.IsElectronicPart) {
                Part part = new Part {
                    Type = Models.Enums.PartType.Electronic,
                    Quantity = repairTaskInputModel.ElectronicPartAmount
                };
                repairTask.PartsRequired.Add(part);
            }
            if (repairTaskInputModel.IsInteriorPart) {
                Part part = new Part {
                    Type = Models.Enums.PartType.Interior,
                    Quantity = repairTaskInputModel.InteriorPartAmount
                };
            }
            this.dbContext.RepairTask.Add(repairTask);
            if(await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
            return repairTask.Id;
        }

        public RepairTask GetById(int id) {
            RepairTask repairTask = this.dbContext.RepairTask.FirstOrDefault(t => t.Id == id);
            return repairTask;
        }

        public IQueryable<RepairTask> GetAllPending() {
            return this.dbContext
                .RepairTask
                .Where(t => t.Status == Models.Enums.Status.Pending);
        }

        public IQueryable<RepairTask> GetAllWorkedByATechnician(string technicianName) {
            return this.dbContext
                .UsersRepairsTasks
                .Where(userRepairTask => userRepairTask.UserId == this.userManager
                                                                    .FindByNameAsync(technicianName)
                                                                    .GetAwaiter()
                                                                    .GetResult()
                                                                    .Id)
                .Select(technician => technician.RepairTask);
        }

        public IQueryable<RepairTask> GetAllWorkedOn() {
            return this.dbContext
                .RepairTask
                .Where(repairTask => repairTask.Status == Models.Enums.Status.WorkedOn);
        }

        public IQueryable<RepairTask> GetAllFinishedRepairTasks() {
            return this.dbContext
                .UsersRepairsTasks
                .Where(usersRepairsTasks => usersRepairsTasks.RepairTask.Status == Models.Enums.Status.Finished)
                .Select(filteredRepairTasks => filteredRepairTasks.RepairTask);
        }
    }
}
