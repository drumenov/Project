using Project.Common.Constants;
using Project.Models.Entities;
using Project.Models.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Project.Services.Tests
{
    public class TechnicianServiceTests : Base
    {
        [Fact]
        public async Task TechniciansSuccessfullyAddedToRepairTask() {
            RepairTask repairTask = new RepairTask { Id = 1, Status = Status.Pending };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();
            User[] technicians = {
                new User { UserName = "test" },
                new User { UserName = "test2" },
                new User { UserName = "test3" }
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
            }
            string[] technicianNames = { "test", "test2", "test3" };
            await this.TechnicianService.AddTechniciansToRepairTaskAsync(technicianNames, 1);
            Assert.Equal(technicians.Length, repairTask.Technicians.Count);
        }

        [Fact]
        public async Task RepairTaskStatusChanged() {
            RepairTask repairTask = new RepairTask { Id = 1, Status = Status.Pending };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();
            User[] technicians = {
                new User { UserName = "test" },
                new User { UserName = "test2" },
                new User { UserName = "test3" }
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
            }
            string[] technicianNames = { "test", "test2", "test3" };
            await this.TechnicianService.AddTechniciansToRepairTaskAsync(technicianNames, 1);
            Assert.Equal(Status.WorkedOn, repairTask.Status);
        }

        [Fact]
        public async Task AllTechnicianAreReturnedSinceAllAreAvailable() {
            User[] technicians = {
                new User { UserName = "test" },
                new User { UserName = "test2" },
                new User { UserName = "test3" }
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
            }
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            foreach (User technicican in technicians) {
                await this.UserManager.AddToRoleAsync(technicican, StringConstants.NoviceTechnicianUserRole);
            }
            Assert.Equal(technicians.Length, this.TechnicianService.GetAllAvailableTechnicians().GetAwaiter().GetResult().Length);
        }

        [Fact]
        public async Task OnlyUsersThatAreTechniciansAndAreAvailableAreReturned() {
            User[] technicians = {
                new User { UserName = "test" },
                new User { UserName = "test2" },
                new User { UserName = "test3" }
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
            }
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            for (int i = 0; i < technicians.Length - 1; i++) {
                await this.UserManager.AddToRoleAsync(technicians[i], StringConstants.NoviceTechnicianUserRole);
            }
            Assert.Equal(technicians.Length - 1, this.TechnicianService.GetAllAvailableTechnicians().GetAwaiter().GetResult().Length);
        }

        [Fact]
        public async Task OnlyAvailableTechniciansAreReturned() {
            User[] technicians = {
                new User { UserName = "test" },
                new User { UserName = "test2" },
                new User { UserName = "test3" }
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
            }
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            foreach (User technicican in technicians) {
                await this.UserManager.AddToRoleAsync(technicican, StringConstants.NoviceTechnicianUserRole);
            }
            UserRepairTask[] userRepairTask = new UserRepairTask[6];
            for (int i = 0; i < userRepairTask.Length; i++) {
                userRepairTask[i] = new UserRepairTask();
                userRepairTask[i].Expert = technicians[0];
                userRepairTask[i].RepairTask = new RepairTask { Id = i + 1 };
            }
            this.dbContext.AddRange(userRepairTask);
            this.dbContext.SaveChanges();
            Assert.Equal(technicians.Length - 1, this.TechnicianService.GetAllAvailableTechnicians().GetAwaiter().GetResult().Length);
        }

        [Fact]
        public async Task AllFinishedRepairTasksOfATechnicianAreReturned() {
            User[] technicians = {
                new User { UserName = "test" },
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
            }
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            foreach (User technicican in technicians) {
                await this.UserManager.AddToRoleAsync(technicican, StringConstants.NoviceTechnicianUserRole);
            }
            UserRepairTask[] userRepairTask = new UserRepairTask[6];
            for (int i = 0; i < userRepairTask.Length; i++) {
                userRepairTask[i] = new UserRepairTask();
                userRepairTask[i].Expert = technicians[0];
                userRepairTask[i].RepairTask = new RepairTask { Id = i + 1 };
            }
            int countOfRepairTasksToBeReturned = 3;
            for (int i = 0; i < countOfRepairTasksToBeReturned; i++) {
                userRepairTask[i].IsFinished = true;
            }
            this.dbContext.AddRange(userRepairTask);
            this.dbContext.SaveChanges();
            Assert.Equal(countOfRepairTasksToBeReturned, this.TechnicianService.GetAllFinishedRepairTaskPerTechnician(technicians[0].UserName).ToArray().Length);
        }

        [Fact]
        public async Task AllTechniciansNotWorkingOnAGivenTaskAreReturned() {
            User[] technicians = {
                new User { UserName = "test" },
                new User { UserName = "test2" },
                new User { UserName = "test3" }
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
            }
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            foreach (User technicican in technicians) {
                await this.UserManager.AddToRoleAsync(technicican, StringConstants.NoviceTechnicianUserRole);
            }
            UserRepairTask userRepairTask = new UserRepairTask {
                Expert = technicians[0],
                RepairTask = new RepairTask { Id = 1 }
            };
            this.dbContext.Add(userRepairTask);
            this.dbContext.SaveChanges();
            Assert.Equal(technicians.Length - 1, this.TechnicianService.GetAllNamesOfTechniciansNotWorkingOnAGivenTask(1).GetAwaiter().GetResult().ToArray().Length);
        }

        [Fact]
        public async Task AllTechniciansAreReturnedBecauseNobodyIsWorkingOnTheGivenTask() {
            User[] technicians = {
                new User { UserName = "test" },
                new User { UserName = "test2" },
                new User { UserName = "test3" }
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
            }
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            foreach (User technicican in technicians) {
                await this.UserManager.AddToRoleAsync(technicican, StringConstants.NoviceTechnicianUserRole);
            }
            UserRepairTask userRepairTask = new UserRepairTask {
                RepairTask = new RepairTask { Id = 1 }
            };
            this.dbContext.Add(userRepairTask);
            this.dbContext.SaveChanges();
            Assert.Equal(technicians.Length, this.TechnicianService.GetAllNamesOfTechniciansNotWorkingOnAGivenTask(1).GetAwaiter().GetResult().ToArray().Length);
        }

        [Fact]
        public async Task AllTechniciansThatAreWorkingOnAGivenTaskAreReturned() {
            User[] technicians = {
                new User { UserName = "test" },
                new User { UserName = "test2" },
                new User { UserName = "test3" }
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
            }
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            foreach (User technicican in technicians) {
                await this.UserManager.AddToRoleAsync(technicican, StringConstants.NoviceTechnicianUserRole);
            }
            UserRepairTask[] userRepairTask = new UserRepairTask[2];
            for (int i = 0; i < 2; i++) {
                userRepairTask[i] = new UserRepairTask {
                    Expert = technicians[i],
                    RepairTask = new RepairTask { Id = 1 }
                };
            }
            this.dbContext.AddRange(userRepairTask);
            this.dbContext.SaveChanges();
            Assert.Equal(technicians.Length - 1, this.TechnicianService.GetAllNamesOfTechniciansWorkingOnAGivenTask(1).ToArray().Length);
        }

        [Fact]
        public async Task NoTechniciansAreReturnedSinceNobodyIsWorkingOnTheGivenRepairTask() {
            User[] technicians = {
                new User { UserName = "test" },
                new User { UserName = "test2" },
                new User { UserName = "test3" }
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
            }
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            foreach (User technicican in technicians) {
                await this.UserManager.AddToRoleAsync(technicican, StringConstants.NoviceTechnicianUserRole);
            }
            UserRepairTask userRepairTask = new UserRepairTask { RepairTaskId = 1 };
            this.dbContext.Add(userRepairTask);
            this.dbContext.SaveChanges();
            Assert.Empty(this.TechnicianService.GetAllNamesOfTechniciansWorkingOnAGivenTask(1).ToArray());
        }

        [Fact]
        public async Task TechnicianIsPromoted() {
            User technician = new User { UserName = "test" };
            await this.UserManager.CreateAsync(technician);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            await this.UserManager.AddToRoleAsync(technician, StringConstants.NoviceTechnicianUserRole);
            await this.TechnicianService.PromoteTechnicianAsync(technician.UserName);
            Assert.True(await this.UserManager.IsInRoleAsync(technician, StringConstants.AverageTechnicianUserRole));
        }

        [Fact]
        public async Task RoleIsNotChangedIfTechnicinIsAlreadyOnMaxLevel() {
            User technician = new User { UserName = "test" };
            await this.UserManager.CreateAsync(technician);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.ExpertTechnicianUserRole });
            await this.UserManager.AddToRoleAsync(technician, StringConstants.ExpertTechnicianUserRole);
            await this.TechnicianService.PromoteTechnicianAsync(technician.UserName);
            Assert.True(await this.UserManager.IsInRoleAsync(technician, StringConstants.ExpertTechnicianUserRole));
        }

        [Fact]
        public async Task TechnicianIsDemoted() {
            User technician = new User { UserName = "test" };
            await this.UserManager.CreateAsync(technician);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.ExpertTechnicianUserRole });
            await this.UserManager.AddToRoleAsync(technician, StringConstants.ExpertTechnicianUserRole);
            await this.TechnicianService.DemoteTechnicianAsync(technician.UserName);
            Assert.True(await this.UserManager.IsInRoleAsync(technician, StringConstants.AdvancedTechnicianUserRole));
        }

        [Fact]
        public async Task RoleRemainTheSameIfTechnicianisAlreadyOnTheLowestLevel() {
            User technician = new User { UserName = "test" };
            await this.UserManager.CreateAsync(technician);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            await this.UserManager.AddToRoleAsync(technician, StringConstants.NoviceTechnicianUserRole);
            await this.TechnicianService.DemoteTechnicianAsync(technician.UserName);
            Assert.True(await this.UserManager.IsInRoleAsync(technician, StringConstants.NoviceTechnicianUserRole));
        }
    }
}
