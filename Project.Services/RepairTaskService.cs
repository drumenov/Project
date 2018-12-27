﻿using Microsoft.AspNetCore.Identity;
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
        private readonly IPartService partService;

        public RepairTaskService(ApplicationDbContext dbContext,
            UserManager<User> userManager,
            IPartService partService) {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.partService = partService;
        }

        public async Task<int> CreateRepairTaskAsync(RepairTaskInputModel repairTaskInputModel, User user) {

            RepairTask repairTask = new RepairTask {
                Status = Models.Enums.Status.Pending,
                User = user
            };
            if (repairTaskInputModel.IsCarBodyPart) {
                if(this.partService.PartTypeExists(Models.Enums.PartType.CarBody) == false) {
                    throw new ArgumentNullException();
                }
                Part part = new Part {
                    Quantity = repairTaskInputModel.CarBodyPartAmount,
                    Type = Models.Enums.PartType.CarBody
                };
                repairTask.PartsRequired.Add(part);
            }
            if (repairTaskInputModel.IsChassisPart) {
                if (this.partService.PartTypeExists(Models.Enums.PartType.Chassis) == false) {
                    throw new ArgumentNullException();
                }
                Part part = new Part {
                    Type = Models.Enums.PartType.Chassis,
                    Quantity = repairTaskInputModel.ChassisPartAmount
                };
                repairTask.PartsRequired.Add(part);
            }
            if (repairTaskInputModel.IsElectronicPart) {
                if (this.partService.PartTypeExists(Models.Enums.PartType.Electronic) == false) {
                    throw new ArgumentNullException();
                }
                Part part = new Part {
                    Type = Models.Enums.PartType.Electronic,
                    Quantity = repairTaskInputModel.ElectronicPartAmount
                };
                repairTask.PartsRequired.Add(part);
            }
            if (repairTaskInputModel.IsInteriorPart) {
                if (this.partService.PartTypeExists(Models.Enums.PartType.Interior) == false) {
                    throw new ArgumentNullException();
                }
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
            string technicianId = this.userManager.FindByNameAsync(technicianName).GetAwaiter().GetResult().Id;
            return this.dbContext
                .UsersRepairsTasks
                .Where(userRepairTask => userRepairTask.UserId == technicianId && userRepairTask.IsFinished == false)
                .Select(technician => technician.RepairTask);
        }

        public IQueryable<RepairTask> GetAllWorkedOn() {
            return this.dbContext
                .RepairTask
                .Where(repairTask => repairTask.Status == Models.Enums.Status.WorkedOn);
        }

        public IQueryable<RepairTask> GetAllFinished() {
            return this.dbContext
                .UsersRepairsTasks
                .Where(usersRepairsTasks => usersRepairsTasks.RepairTask.Status == Models.Enums.Status.Finished)
                .Select(filteredRepairTasks => filteredRepairTasks.RepairTask);
        }

        public async Task<IQueryable<RepairTask>> GetPendingPerCustomerAsync(string customerName) {
            string customerId = this.userManager.FindByNameAsync(customerName).GetAwaiter().GetResult().Id;
            return this.dbContext
                .RepairTask
                .Where(repairTask => repairTask.UserId == customerId);
        }

        public async Task<IQueryable<RepairTask>> GetWorkedOnPerCustomerAsync(string customerName) {
            string customerId = this.userManager.FindByNameAsync(customerName).GetAwaiter().GetResult().Id;
            return this.dbContext
                .RepairTask
                .Where(repairTask => repairTask.UserId == customerId && repairTask.Status == Models.Enums.Status.WorkedOn);

        }

        public async Task<IQueryable<RepairTask>> GetFinishedPerCustomerAsync(string customerName) {
            Guid customerId = Guid.Parse(this.userManager.FindByNameAsync(customerName).GetAwaiter().GetResult().Id);

            var t = this.dbContext
                .RepairTask
                .Where(repairTask => Guid.Parse(repairTask.User.Id) == customerId && repairTask.Status == Models.Enums.Status.Finished);

            return this.dbContext
                .RepairTask
                .Where(repairTask => Guid.Parse(repairTask.User.Id) == customerId && repairTask.Status == Models.Enums.Status.Finished);
        }

        public IQueryable<User> GetTechniciansHavingWorkedOnARepairTask(int repairTaskId) {
            return this.dbContext
                .UsersRepairsTasks
                .Where(userRepairTask => userRepairTask.RepairTaskId == repairTaskId)
                .Select(filteredUserRepairTask => filteredUserRepairTask.Expert);
        }

        public async Task TechnicianCompletesARepairTaskAsync(int repairTaskId, string technicianName) {
            string technicianId = this.userManager.FindByNameAsync(technicianName).GetAwaiter().GetResult().Id;
            this.dbContext
                .UsersRepairsTasks
                .Where(userRepairTask => userRepairTask.UserId == technicianId && userRepairTask.RepairTaskId == repairTaskId)
                .FirstOrDefault().IsFinished = true;
            
            if(await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
            await TryCompoleteRepairTaskAsync(repairTaskId);
        }

        private async Task TryCompoleteRepairTaskAsync(int repairTaskId) {
            if(this.dbContext
                .UsersRepairsTasks
                .All(userRepairTask => userRepairTask.IsFinished)) {
                this.dbContext
                    .RepairTask
                    .FirstOrDefault(repairTask => repairTask.Id == repairTaskId)
                    .Status = Models.Enums.Status.Finished;
                if(await this.dbContext.SaveChangesAsync() == 0) {
                    throw new ApplicationException();
                }
                //TODO: Create receipt here.
            }
        }

        public async Task RemoveTechnicianFromRepairTaskAsync(string nameOfTechnicianToRemove, int id) {
            string technicianId = this.userManager.FindByNameAsync(nameOfTechnicianToRemove).GetAwaiter().GetResult().Id;
            UserRepairTask repairTaskToRemove = this.dbContext
                                                .UsersRepairsTasks
                                                .Where(usersRepairTasks => usersRepairTasks.Expert.Id == technicianId && usersRepairTasks.RepairTaskId == id)
                                                .FirstOrDefault();
            if(repairTaskToRemove == null) {
                throw new ArgumentNullException();
            }
            this.dbContext
                .UsersRepairsTasks
                .Remove(repairTaskToRemove);
            if(await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
            await this.CheckWhetherThereAreAnyTechnicianStillAssignedToTheTaskAsync(id);
        }

        private async Task CheckWhetherThereAreAnyTechnicianStillAssignedToTheTaskAsync(int id) {
            if (this.dbContext
                    .RepairTask
                    .Where(repairTask => repairTask.Id == id)
                    .SelectMany(filteredRepairTasks => filteredRepairTasks.Technicians)
                    .Count() == 0) { //This check whether there are any technicians working on the repair task. If all have been removed, the status of the repair task must be changed back to pending
                RepairTask currentRepairTask = this.dbContext.RepairTask.First(repairTask => repairTask.Id == id);
                currentRepairTask.Status = Models.Enums.Status.Pending;
            }
            if(await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
        }

        public async Task AddTechnicianToRepairTaskAsync(string nameOfTechnicianToAdd, int id) {
            string userId = this.userManager.FindByNameAsync(nameOfTechnicianToAdd).GetAwaiter().GetResult().Id;
            UserRepairTask userRepairTask = new UserRepairTask {
                UserId = userId,
                RepairTaskId = id
            };
            if(await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
        }
    }
}
