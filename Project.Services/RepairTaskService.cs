using Microsoft.AspNetCore.Identity;
using Project.Data;
using Project.Models.Entities;
using Project.Models.Enums;
using Project.Models.InputModels.Customer;
using Project.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Services
{
    public class RepairTaskService : IRepairTaskService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly IPartService partService;
        private readonly IReceiptService receiptService;

        public RepairTaskService(ApplicationDbContext dbContext,
            UserManager<User> userManager,
            IPartService partService,
            IReceiptService receiptService) {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.partService = partService;
            this.receiptService = receiptService;
        }

        public async Task<int> CreateRepairTaskAsync(RepairTaskInputModel repairTaskInputModel, User user) {

            RepairTask repairTask = new RepairTask {
                Status = Status.Pending,
                User = user
            };
            if (repairTaskInputModel.IsCarBodyPart) {
                if (this.partService.PartTypeExists(PartType.CarBody) == false) {
                    throw new ArgumentNullException();
                }
                Part part = new Part {
                    Quantity = repairTaskInputModel.CarBodyPartAmount,
                    Type = PartType.CarBody
                };
                repairTask.PartsRequired.Add(part);
            }
            if (repairTaskInputModel.IsChassisPart) {
                if (this.partService.PartTypeExists(PartType.Chassis) == false) {
                    throw new ArgumentNullException();
                }
                Part part = new Part {
                    Type = PartType.Chassis,
                    Quantity = repairTaskInputModel.ChassisPartAmount
                };
                repairTask.PartsRequired.Add(part);
            }
            if (repairTaskInputModel.IsElectronicPart) {
                if (this.partService.PartTypeExists(PartType.Electronic) == false) {
                    throw new ArgumentNullException();
                }
                Part part = new Part {
                    Type = PartType.Electronic,
                    Quantity = repairTaskInputModel.ElectronicPartAmount
                };
                repairTask.PartsRequired.Add(part);
            }
            if (repairTaskInputModel.IsInteriorPart) {
                if (this.partService.PartTypeExists(PartType.Interior) == false) {
                    throw new ArgumentNullException();
                }
                Part part = new Part {
                    Type = PartType.Interior,
                    Quantity = repairTaskInputModel.InteriorPartAmount
                };
                repairTask.PartsRequired.Add(part);
            }
            this.dbContext.RepairTasks.Add(repairTask);
            if (await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
            return repairTask.Id;
        }

        public RepairTask GetById(int id) {
            RepairTask repairTask = this.dbContext.RepairTasks.FirstOrDefault(t => t.Id == id);
            return repairTask;
        }

        public IQueryable<RepairTask> GetAllPending() {
            return this.dbContext
                .RepairTasks
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
                .RepairTasks
                .Where(repairTask => repairTask.Status == Status.WorkedOn);
        }

        public IQueryable<RepairTask> GetAllFinished() {
            return this.dbContext
                .RepairTasks
                .Where(repairTask => repairTask.Status == Models.Enums.Status.Finished);
        }

        public async Task<IQueryable<RepairTask>> GetPendingPerCustomerAsync(string customerName) {
            string customerId = this.userManager.FindByNameAsync(customerName).GetAwaiter().GetResult().Id;
            return this.dbContext
                .RepairTasks
                .Where(repairTask => repairTask.UserId == customerId && repairTask.Status == Models.Enums.Status.Pending);
        }

        public async Task<IQueryable<RepairTask>> GetWorkedOnPerCustomerAsync(string customerName) {
            string customerId = this.userManager.FindByNameAsync(customerName).GetAwaiter().GetResult().Id;
            return this.dbContext
                .RepairTasks
                .Where(repairTask => repairTask.UserId == customerId && repairTask.Status == Models.Enums.Status.WorkedOn);

        }

        public async Task<IQueryable<RepairTask>> GetFinishedPerCustomerAsync(string customerName) {
            string customerId = this.userManager.FindByNameAsync(customerName).GetAwaiter().GetResult().Id;
            return this.dbContext
                .RepairTasks
                .Where(repairTask => repairTask.User.Id == customerId && repairTask.Status == Models.Enums.Status.Finished);
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

            if (await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
            await TryCompoleteRepairTaskAsync(repairTaskId);
        }

        private async Task TryCompoleteRepairTaskAsync(int repairTaskId) {
            if (this.dbContext
                .UsersRepairsTasks
                .Where(userRepairTask => userRepairTask.RepairTaskId == repairTaskId)
                .All(userRepairTask => userRepairTask.IsFinished)) {
                RepairTask currentRepairTask = this.dbContext
                    .RepairTasks
                    .FirstOrDefault(repairTask => repairTask.Id == repairTaskId);
                currentRepairTask.Status = Models.Enums.Status.Finished;
                if (await this.dbContext.SaveChangesAsync() == 0) {
                    throw new ApplicationException();
                }
                string customerId = currentRepairTask.UserId;
                await this.receiptService.GenerateReceiptAsync(this.GetTechniciansHavingWorkedOnARepairTask(repairTaskId).ToArray(), customerId, currentRepairTask);
                //TODO: Create receipt here.
            }
        }

        public async Task RemoveTechnicianFromRepairTaskAsync(string nameOfTechnicianToRemove, int id) {
            string technicianId = this.userManager.FindByNameAsync(nameOfTechnicianToRemove).GetAwaiter().GetResult().Id;
            UserRepairTask repairTaskToRemove = this.dbContext
                                                .UsersRepairsTasks
                                                .Where(usersRepairTasks => usersRepairTasks.Expert.Id == technicianId && usersRepairTasks.RepairTaskId == id)
                                                .FirstOrDefault();
            if (repairTaskToRemove == null) {
                throw new ArgumentNullException();
            }
            this.dbContext
                .UsersRepairsTasks
                .Remove(repairTaskToRemove);
            if (await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
            await this.CheckWhetherThereAreAnyTechnicianStillAssignedToTheTaskAsync(id);
        }

        private async Task CheckWhetherThereAreAnyTechnicianStillAssignedToTheTaskAsync(int id) {
            if (this.dbContext
                    .RepairTasks
                    .Where(repairTask => repairTask.Id == id)
                    .SelectMany(filteredRepairTasks => filteredRepairTasks.Technicians)
                    .Count() == 0) { //This check whether there are any technicians working on the repair task. If all have been removed, the status of the repair task must be changed back to pending
                RepairTask currentRepairTask = this.dbContext.RepairTasks.First(repairTask => repairTask.Id == id);
                currentRepairTask.Status = Status.Pending;
                if (await this.dbContext.SaveChangesAsync() == 0) {
                    throw new ApplicationException();
                }
            }
        }

        public async Task AddTechnicianToRepairTaskAsync(string nameOfTechnicianToAdd, int id) {
            string userId = this.userManager.FindByNameAsync(nameOfTechnicianToAdd).GetAwaiter().GetResult().Id;
            UserRepairTask userRepairTask = new UserRepairTask {
                UserId = userId,
                RepairTaskId = id
            };
            await this.dbContext
                .UsersRepairsTasks
                .AddAsync(userRepairTask);
            if (await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
        }

        public IQueryable<RepairTask> GetAllPerCustomer(string customerName) {
            return this.dbContext.RepairTasks.Where(repairTask => repairTask.User.UserName.Equals(customerName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<bool> TryUpdateRepairTaskAsync(RepairTaskEditInputModel repairTaskEditInputModel) {
            RepairTask repairTask = this.GetById(repairTaskEditInputModel.Id);
            if (repairTaskEditInputModel.IsCarBodyPart) {
                if (this.partService.PartTypeExists(PartType.CarBody) == false) {
                    throw new ApplicationException();
                }
                Part part = this.GetPartByPartTypeAndRepairTaskId(PartType.CarBody, repairTask.Id);
                if (part == null) {
                    part = new Part {
                        Type = PartType.CarBody,
                    };
                    repairTask.PartsRequired.Add(part);
                }
                part.Quantity = repairTaskEditInputModel.CarBodyPartAmount; 
            }
            if (repairTaskEditInputModel.IsChassisPart) {
                if (this.partService.PartTypeExists(PartType.Chassis) == false) {
                    throw new ArgumentNullException();
                }
                Part part = this.GetPartByPartTypeAndRepairTaskId(PartType.Chassis, repairTask.Id);
                if(part == null) {
                    part = new Part {
                        Type = PartType.Chassis,
                    };
                    repairTask.PartsRequired.Add(part);
                }
                part.Quantity = repairTaskEditInputModel.ChassisPartAmount;                
            }
            if (repairTaskEditInputModel.IsElectronicPart) {
                if (this.partService.PartTypeExists(PartType.Electronic) == false) {
                    throw new ArgumentNullException();
                }
                Part part = this.GetPartByPartTypeAndRepairTaskId(PartType.Electronic, repairTask.Id);
                if(part == null) {
                    part = new Part {
                        Type = PartType.Electronic,
                    };
                    repairTask.PartsRequired.Add(part);
                };
                part.Quantity = repairTaskEditInputModel.ElectronicPartAmount;
            }
            if (repairTaskEditInputModel.IsInteriorPart) {
                if (this.partService.PartTypeExists(PartType.Interior) == false) {
                    throw new ArgumentNullException();
                }
                Part part = this.GetPartByPartTypeAndRepairTaskId(PartType.Interior, repairTask.Id);
                if(part == null) {
                    part = new Part {
                        Type = PartType.Interior,
                    };
                    repairTask.PartsRequired.Add(part);
                };
                part.Quantity = repairTaskEditInputModel.InteriorPartAmount;
            }

            //for(int i = 0; i < repairTask.PartsRequired.ToList().Count; i++) {
            //    Part part = repairTask.PartsRequired.ToList()[i];
            //    if (part.Quantity == 0) {
            //        repairTask.PartsRequired.Remove(part);
            //    }
            //}
            //foreach (Part part in repairTask.PartsRequired.ToList()) {
            //    if (part.Quantity == 0) {
            //        repairTask.PartsRequired.Remove(part);
            //    }
            //}

            if (repairTask.PartsRequired.All(x => x.Quantity == 0)) {
                return false;
            } else {
                this.dbContext.Parts.RemoveRange(repairTask.PartsRequired.Where(x => x.Quantity == 0));
            }

            if (await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
            return true;
        }

        private Part GetPartByPartTypeAndRepairTaskId(PartType partType, int id) {
            return this.dbContext
                    .RepairTasks
                    .Where(rt => rt.Id == id)
                    .SelectMany(filtered => filtered.PartsRequired)
                    .Where(p => p.Type == partType)
                    .FirstOrDefault();
        }

        public bool RepairTaskIsChanged(RepairTaskEditInputModel repairTaskEditInputModel) {
            RepairTask repairTask = this.GetById(repairTaskEditInputModel.Id);
            List<Part> repairTaskOldRequiredParts = new List<Part>(repairTask.PartsRequired).OrderBy(x => x.Type).ToList();
            List<Part> repairTaskNewRequiredParts = new List<Part>();
            if (repairTaskEditInputModel.IsCarBodyPart) {
                repairTaskNewRequiredParts.Add(new Part {
                    Type = PartType.CarBody,
                    Quantity = repairTaskEditInputModel.CarBodyPartAmount
                });
            }
            if (repairTaskEditInputModel.IsChassisPart) {
                repairTaskNewRequiredParts.Add(new Part {
                    Type = PartType.Chassis,
                    Quantity = repairTaskEditInputModel.ChassisPartAmount
                });
            }
            if (repairTaskEditInputModel.IsElectronicPart) {
                repairTaskNewRequiredParts.Add(new Part {
                    Type = PartType.Electronic,
                    Quantity = repairTaskEditInputModel.ElectronicPartAmount
                });
            }
            if (repairTaskEditInputModel.IsInteriorPart) {
                repairTaskNewRequiredParts.Add(new Part {
                    Type = PartType.Interior,
                    Quantity = repairTaskEditInputModel.InteriorPartAmount
                });
            }
            repairTaskNewRequiredParts = repairTaskNewRequiredParts.OrderBy(x => x.Type).ToList();
            if (repairTaskOldRequiredParts.Count != repairTaskNewRequiredParts.Count) {
                return true;
            }
            else {
                for (int i = 0; i < repairTaskOldRequiredParts.Count; i++) {
                    if (repairTaskOldRequiredParts[i].Quantity != repairTaskNewRequiredParts[i].Quantity) {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task DeleteRepairTaskAsync(int id) {
            RepairTask repairTask = this.GetById(id);
            if(repairTask.Status != Status.Pending) {
                throw new ApplicationException();
            }
            Part[] partsToRemove = repairTask.PartsRequired.ToArray();
            this.dbContext
                .Parts.RemoveRange(partsToRemove);
            this.dbContext
                .RepairTasks
                .Remove(repairTask);

            if (await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
        }
    }
}
