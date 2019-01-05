using Project.Common.Constants;
using Project.Models.Entities;
using Project.Models.Enums;
using Project.Models.InputModels.Customer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Project.Services.Tests.TestsForRepairTasksService
{
    public class RepairTaskServiceTests : Base
    {
        [Fact]
        public void AllPendingRepairTasksAreReturned() {
            RepairTask[] repairTasks = {
                new RepairTask{ Status = Status.Pending},
                new RepairTask{Status = Status.Pending},
                new RepairTask { Status = Status.Pending}
            };
            this.dbContext.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length, this.RepairTaskService.GetAllPending().ToArray().Length);
        }

        [Fact]
        public void OnlyPendingRepairTasksAreReturned() {
            RepairTask[] repairTasks = {
                new RepairTask{ Status = Status.Pending},
                new RepairTask{Status = Status.Pending},
                new RepairTask { Status = Status.Finished}
            };
            this.dbContext.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length - 1, this.RepairTaskService.GetAllPending().ToArray().Length);
        }

        [Fact]
        public void NoPendingRepairTasksAreReturnedSinceThereAreNone() {
            RepairTask[] repairTasks = {
                new RepairTask{ Status = Status.Finished},
                new RepairTask{Status = Status.Finished},
                new RepairTask { Status = Status.Finished}
            };
            this.dbContext.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Empty(this.RepairTaskService.GetAllPending());
        }

        [Fact]
        public async Task AllRepairTasksWorkedByATechnicianAreReturned() {
            User technician = new User { UserName = "test" };
            await this.UserManager.CreateAsync(technician);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.ExpertTechnicianUserRole });
            await this.UserManager.AddToRoleAsync(technician, StringConstants.ExpertTechnicianUserRole);
            UserRepairTask[] userRepairTask = new UserRepairTask[3];
            for (int i = 0; i < userRepairTask.Length; i++) {
                userRepairTask[i] = new UserRepairTask {
                    Expert = technician,
                    IsFinished = false,
                    RepairTask = new RepairTask { Id = i + 1 }
                };
            }
            this.dbContext.AddRange(userRepairTask);
            this.dbContext.SaveChanges();
            Assert.Equal(userRepairTask.Length, this.RepairTaskService.GetAllWorkedByATechnician(technician.UserName).ToArray().Length);
        }

        [Fact]
        public async Task OnlyTheWorkedOnRepaiTasksByTechnicianAreReturned() {
            User technician = new User { UserName = "test" };
            await this.UserManager.CreateAsync(technician);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.ExpertTechnicianUserRole });
            await this.UserManager.AddToRoleAsync(technician, StringConstants.ExpertTechnicianUserRole);
            UserRepairTask[] userRepairTask = new UserRepairTask[3];
            for (int i = 0; i < userRepairTask.Length; i++) {
                userRepairTask[i] = new UserRepairTask {
                    Expert = technician,
                    RepairTask = new RepairTask { Id = i + 1 }
                };
            }
            userRepairTask[0].IsFinished = true;
            this.dbContext.AddRange(userRepairTask);
            this.dbContext.SaveChanges();
            Assert.Equal(userRepairTask.Length - 1, this.RepairTaskService.GetAllWorkedByATechnician(technician.UserName).ToArray().Length);
        }

        [Fact]
        public async Task NoRepairTasksAreReturnedSinceTheTasksOfTheTechnicianAreAllFinished() {
            User technician = new User { UserName = "test" };
            await this.UserManager.CreateAsync(technician);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.ExpertTechnicianUserRole });
            await this.UserManager.AddToRoleAsync(technician, StringConstants.ExpertTechnicianUserRole);
            UserRepairTask[] userRepairTask = new UserRepairTask[3];
            for (int i = 0; i < userRepairTask.Length; i++) {
                userRepairTask[i] = new UserRepairTask {
                    Expert = technician,
                    IsFinished = true,
                    RepairTask = new RepairTask { Id = i + 1 }
                };
            }
            this.dbContext.AddRange(userRepairTask);
            this.dbContext.SaveChanges();
            Assert.Empty(this.RepairTaskService.GetAllWorkedByATechnician(technician.UserName));
        }

        [Fact]
        public void GetAllRepairTasksSinceTheyAreAllWorkedOn() {
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask { Status = Status.WorkedOn };
            }
            this.dbContext.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length, this.RepairTaskService.GetAllWorkedOn().ToArray().Length);
        }

        [Fact]
        public void GetSomeOfTheRepairTasksSinceNotAllAreWorkedOn() {
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask { Status = Status.WorkedOn };
            }
            repairTasks[0].Status = Status.Finished;
            this.dbContext.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length - 1, this.RepairTaskService.GetAllWorkedOn().ToArray().Length);
        }

        [Fact]
        public void NoTasksAreReturnedSinceNoTaskIsWorkedOn() {
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask { Status = Status.Finished };
            }
            this.dbContext.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Empty(this.RepairTaskService.GetAllWorkedOn());
        }

        [Fact]
        public void GetAllRepairTasksSinceTheyAreAllFinished() {
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask { Status = Status.Finished };
            }
            this.dbContext.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length, this.RepairTaskService.GetAllFinished().ToArray().Length);
        }

        [Fact]
        public void GetSomeOfTheRepairTasksSinceNotAllAreFinished() {
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask { Status = Status.Finished };
            }
            repairTasks[0].Status = Status.WorkedOn;
            this.dbContext.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length - 1, this.RepairTaskService.GetAllFinished().ToArray().Length);
        }

        [Fact]
        public void NoTasksAreReturnedSinceNoTaskIsFinished() {
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask { Status = Status.WorkedOn };
            }
            this.dbContext.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Empty(this.RepairTaskService.GetAllFinished());
        }

        [Fact]
        public async Task GetAllPendingRepairTaskFilteredByCustomer() {
            User customer = new User { UserName = "test" };
            await this.UserManager.CreateAsync(customer);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customer,
                    Status = Status.Pending
                };
            }
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length, this.RepairTaskService.GetPendingPerCustomerAsync(customer.UserName).GetAwaiter().GetResult().ToArray().Length);
        }

        [Fact]
        public async Task GetSomeRepairTasksFilteredPerCustomerSinceNotAllRepairTasksArePending() {
            User customer = new User { UserName = "test" };
            await this.UserManager.CreateAsync(customer);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customer,
                    Status = Status.Pending
                };
            }
            repairTasks[0].Status = Status.Finished;
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length - 1, this.RepairTaskService.GetPendingPerCustomerAsync(customer.UserName).GetAwaiter().GetResult().ToArray().Length);
        }

        [Fact]
        public async Task GetSomePendingRepairTasksFilteredPerCustomerSinceNotAllAreForTheSameCustomer() {
            User[] customers = {
                new User  {UserName = "test" },
                new User {UserName = "test2"}
            };
            foreach (User customer in customers) {
                await this.UserManager.CreateAsync(customer);
            }
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            foreach (User customer in customers) {
                await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            }
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customers[0],
                    Status = Status.Pending
                };
            }
            repairTasks[0].User = customers[1];
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length - 1, this.RepairTaskService.GetPendingPerCustomerAsync(customers[0].UserName).GetAwaiter().GetResult().ToArray().Length);
        }

        [Fact]
        public async Task GetNoRepairTasksFilteredPerCustomerSinceThereAreNoneThatArePending() {
            User customer = new User { UserName = "test" };
            await this.UserManager.CreateAsync(customer);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customer,
                    Status = Status.Finished
                };
            }
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Empty(await this.RepairTaskService.GetPendingPerCustomerAsync(customer.UserName));
        }

        [Fact]
        public async Task GetNoRepairTasksFilteredPerCustomerSinceThereAreNoneThatArePendingForThisCustomer() {
            User[] customers = {
                new User  {UserName = "test" },
                new User {UserName = "test2"}
            };
            foreach (User customer in customers) {
                await this.UserManager.CreateAsync(customer);
            }
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            foreach (User customer in customers) {
                await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            }
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customers[1],
                    Status = Status.Pending
                };
            }
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Empty(await this.RepairTaskService.GetPendingPerCustomerAsync(customers[0].UserName));
        }

        [Fact]
        public async Task GetAllFinishedRepairTaskFilteredByCustomer() {
            User customer = new User { UserName = "test" };
            await this.UserManager.CreateAsync(customer);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customer,
                    Status = Status.Finished
                };
            }
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length, this.RepairTaskService.GetFinishedPerCustomerAsync(customer.UserName).GetAwaiter().GetResult().ToArray().Length);
        }

        [Fact]
        public async Task GetSomeRepairTasksFilteredPerCustomerSinceNotAllRepairTasksAreFinished() {
            User customer = new User { UserName = "test" };
            await this.UserManager.CreateAsync(customer);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customer,
                    Status = Status.Finished
                };
            }
            repairTasks[0].Status = Status.Pending;
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length - 1, this.RepairTaskService.GetFinishedPerCustomerAsync(customer.UserName).GetAwaiter().GetResult().ToArray().Length);
        }

        [Fact]
        public async Task GetSomeFinishedRepairTasksFilteredPerCustomerSinceNotAllAreForTheSameCustomer() {
            User[] customers = {
                new User  {UserName = "test" },
                new User {UserName = "test2"}
            };
            foreach (User customer in customers) {
                await this.UserManager.CreateAsync(customer);
            }
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            foreach (User customer in customers) {
                await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            }
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customers[0],
                    Status = Status.Finished
                };
            }
            repairTasks[0].User = customers[1];
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length - 1, this.RepairTaskService.GetFinishedPerCustomerAsync(customers[0].UserName).GetAwaiter().GetResult().ToArray().Length);
        }

        [Fact]
        public async Task GetNoRepairTasksFilteredPerCustomerSinceThereAreNoneThatAreFinished() {
            User customer = new User { UserName = "test" };
            await this.UserManager.CreateAsync(customer);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customer,
                    Status = Status.Pending
                };
            }
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Empty(await this.RepairTaskService.GetFinishedPerCustomerAsync(customer.UserName));
        }

        [Fact]
        public async Task GetNoRepairTasksFilteredPerCustomerSinceThereAreNoneThatAreFinishedForThisCustomer() {
            User[] customers = {
                new User  {UserName = "test" },
                new User {UserName = "test2"}
            };
            foreach (User customer in customers) {
                await this.UserManager.CreateAsync(customer);
            }
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            foreach (User customer in customers) {
                await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            }
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customers[1],
                    Status = Status.Finished
                };
            }
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Empty(await this.RepairTaskService.GetFinishedPerCustomerAsync(customers[0].UserName));
        }

        [Fact]
        public async Task GetAllWorkedOnRepairTaskFilteredByCustomer() {
            User customer = new User { UserName = "test" };
            await this.UserManager.CreateAsync(customer);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customer,
                    Status = Status.WorkedOn
                };
            }
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length, this.RepairTaskService.GetWorkedOnPerCustomerAsync(customer.UserName).GetAwaiter().GetResult().ToArray().Length);
        }

        [Fact]
        public async Task GetSomeRepairTasksFilteredPerCustomerSinceNotAllRepairTasksAreWorkedOn() {
            User customer = new User { UserName = "test" };
            await this.UserManager.CreateAsync(customer);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customer,
                    Status = Status.WorkedOn
                };
            }
            repairTasks[0].Status = Status.Pending;
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length - 1, this.RepairTaskService.GetWorkedOnPerCustomerAsync(customer.UserName).GetAwaiter().GetResult().ToArray().Length);
        }

        [Fact]
        public async Task GetSomeWorkedOnRepairTasksFilteredPerCustomerSinceNotAllAreForTheSameCustomer() {
            User[] customers = {
                new User  {UserName = "test" },
                new User {UserName = "test2"}
            };
            foreach (User customer in customers) {
                await this.UserManager.CreateAsync(customer);
            }
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            foreach (User customer in customers) {
                await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            }
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customers[0],
                    Status = Status.WorkedOn
                };
            }
            repairTasks[0].User = customers[1];
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length - 1, this.RepairTaskService.GetWorkedOnPerCustomerAsync(customers[0].UserName).GetAwaiter().GetResult().ToArray().Length);
        }

        [Fact]
        public async Task GetNoRepairTasksFilteredPerCustomerSinceThereAreNoneThatAreWorkedOn() {
            User customer = new User { UserName = "test" };
            await this.UserManager.CreateAsync(customer);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customer,
                    Status = Status.Pending
                };
            }
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Empty(await this.RepairTaskService.GetWorkedOnPerCustomerAsync(customer.UserName));
        }

        [Fact]
        public async Task GetNoRepairTasksFilteredPerCustomerSinceThereAreNoneThatAreWorkedOnForThisCustomer() {
            User[] customers = {
                new User  {UserName = "test" },
                new User {UserName = "test2"}
            };
            foreach (User customer in customers) {
                await this.UserManager.CreateAsync(customer);
            }
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerUserRole });
            foreach (User customer in customers) {
                await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);
            }
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customers[1],
                    Status = Status.WorkedOn
                };
            }
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Empty(await this.RepairTaskService.GetWorkedOnPerCustomerAsync(customers[0].UserName));
        }

        [Fact]
        public async Task GetAllTechniciansSinceTheyAreLinkedToTheGivenRepairTask() {
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
            UserRepairTask[] userRepairTask = new UserRepairTask[3];
            for (int i = 0; i < userRepairTask.Length; i++) {
                userRepairTask[i] = new UserRepairTask {
                    Expert = technicians[i],
                    RepairTask = new RepairTask { Id = 1 }
                };
            }
            this.dbContext.AddRange(userRepairTask);
            this.dbContext.SaveChanges();
            Assert.Equal(technicians.Length, this.RepairTaskService.GetTechniciansHavingWorkedOnARepairTask(1).ToArray().Length);
        }

        [Fact]
        public async Task GetSomeTechniciansSinceNotAllAreLinkedToTheGivenRepairTask() {
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
            UserRepairTask[] userRepairTask = new UserRepairTask[3];
            for (int i = 0; i < userRepairTask.Length; i++) {
                userRepairTask[i] = new UserRepairTask {
                    Expert = technicians[i],
                    RepairTask = new RepairTask { Id = 1 }
                };
            }
            userRepairTask[0].RepairTask.Id = 2;
            this.dbContext.AddRange(userRepairTask);
            this.dbContext.SaveChanges();
            Assert.Equal(technicians.Length - 1, this.RepairTaskService.GetTechniciansHavingWorkedOnARepairTask(1).ToArray().Length);
        }

        [Fact]
        public async Task GetNoTechniciansSinceNoneAreLinkedToTheGivenRepairTask() {
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
            UserRepairTask[] userRepairTask = new UserRepairTask[3];
            for (int i = 0; i < userRepairTask.Length; i++) {
                userRepairTask[i] = new UserRepairTask {
                    Expert = technicians[i],
                    RepairTask = new RepairTask { Id = 2 }
                };
            }
            this.dbContext.AddRange(userRepairTask);
            this.dbContext.SaveChanges();
            Assert.Empty(this.RepairTaskService.GetTechniciansHavingWorkedOnARepairTask(1));
        }

        [Fact]
        public async Task ATechnicianCanCompleteARepairTask() {
            User technician = new User { UserName = "test" };
            await this.UserManager.CreateAsync(technician);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            await this.UserManager.AddToRoleAsync(technician, StringConstants.NoviceTechnicianUserRole);

            User customer = new User { UserName = "test2" };
            await this.UserManager.CreateAsync(customer);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CorporateCustomerUserRole });
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CorporateCustomerUserRole);

            UserRepairTask userRepairTask = new UserRepairTask {
                RepairTask = new RepairTask { Id = 1, Status = Status.WorkedOn, User = customer },
                IsFinished = false,
                Expert = technician
            };
            this.dbContext.UsersRepairsTasks.Add(userRepairTask);
            this.dbContext.SaveChanges();
            await this.RepairTaskService.TechnicianCompletesARepairTaskAsync(1, technician.UserName);
            Assert.Equal(Status.Finished, userRepairTask.RepairTask.Status);
        }

        [Fact]
        public async Task ATaskCannotBeCompletedIfOneOfTheTechniciansHasNotCompletedIt() {
            User[] technicians = {
                new User { UserName = "test" },
                new User {UserName = "test2"},
                new User {UserName = "test3"}
            };
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            UserRepairTask[] userRepairTasks = new UserRepairTask[3];
            for (int i = 0; i < technicians.Length; i++) {
                await this.UserManager.CreateAsync(technicians[i]);
                await this.UserManager.AddToRoleAsync(technicians[i], StringConstants.NoviceTechnicianUserRole);
                userRepairTasks[i] = new UserRepairTask {
                    RepairTask = new RepairTask { Id = 1, Status = Status.WorkedOn },
                    IsFinished = false,
                    Expert = technicians[i]
                };
            }
            this.dbContext.UsersRepairsTasks.AddRange(userRepairTasks);
            this.dbContext.SaveChanges();
            for (int i = 0; i < technicians.Length - 1; i++) {
                await this.RepairTaskService.TechnicianCompletesARepairTaskAsync(1, technicians[i].UserName);
            }
            Assert.Equal(Status.WorkedOn, userRepairTasks[0].RepairTask.Status);
        }

        [Fact]
        public async Task ARepairTaskIsComletedWhenAllTechniciansCompleteIt() {
            User[] technicians = {
                new User {UserName = "test" },
                new User {UserName = "test2"},
                new User {UserName = "test3"}
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
                await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
                await this.UserManager.AddToRoleAsync(technician, StringConstants.NoviceTechnicianUserRole);
            }
            User customer = new User { UserName = "test4" };
            await this.UserManager.CreateAsync(customer);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CorporateCustomerUserRole });
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CorporateCustomerUserRole);

            UserRepairTask[] userRepairTasks = new UserRepairTask[3];
            for (int i = 0; i < userRepairTasks.Length; i++) {
                userRepairTasks[i] = new UserRepairTask {
                    RepairTask = new RepairTask { Id = 1, Status = Status.WorkedOn, User = customer },
                    IsFinished = false,
                    Expert = technicians[i]
                };
            }
            this.dbContext.UsersRepairsTasks.AddRange(userRepairTasks);
            this.dbContext.SaveChanges();
            foreach (User technician in technicians) {
                await this.RepairTaskService.TechnicianCompletesARepairTaskAsync(1, technician.UserName);
            }
            Assert.Equal(Status.Finished, userRepairTasks[0].RepairTask.Status);
        }

        [Fact]
        public async Task ATechnicianCanBeRemovedFromARepairTask() {
            User[] technicians = {
                new User {UserName = "test" },
                new User {UserName = "test2"},
                new User {UserName = "test3"}
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
                await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
                await this.UserManager.AddToRoleAsync(technician, StringConstants.NoviceTechnicianUserRole);
            }

            UserRepairTask[] userRepairTasks = new UserRepairTask[3];
            for (int i = 0; i < userRepairTasks.Length; i++) {
                userRepairTasks[i] = new UserRepairTask {
                    RepairTask = new RepairTask { Id = 1, Status = Status.WorkedOn },
                    IsFinished = false,
                    Expert = technicians[i]
                };
            }
            this.dbContext.UsersRepairsTasks.AddRange(userRepairTasks);
            this.dbContext.SaveChanges();
            await this.RepairTaskService.RemoveTechnicianFromRepairTaskAsync(technicians[0].UserName, 1);
            Assert.Equal(technicians.Length - 1, userRepairTasks[0].RepairTask.Technicians.Count);
        }

        [Fact]
        public async Task ATechnicianCanBeAddedToARepairTask() {
            User technician = new User { UserName = "test" };
            await this.UserManager.CreateAsync(technician);
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            await this.UserManager.AddToRoleAsync(technician, StringConstants.NoviceTechnicianUserRole);

            RepairTask repairTask = new RepairTask { Id = 1 };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();
            await this.RepairTaskService.AddTechnicianToRepairTaskAsync(technician.UserName, 1);
            Assert.Equal(1, repairTask.Technicians.Count);
        }

        [Fact]
        public async Task RepairTaskStatusChangedToPendingIfAllTechniciansAreRemovedFromRepairTask() {
            User[] technicians = {
                new User {UserName = "test" },
                new User {UserName = "test2"},
                new User {UserName = "test3"}
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
                await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
                await this.UserManager.AddToRoleAsync(technician, StringConstants.NoviceTechnicianUserRole);
            }

            UserRepairTask[] userRepairTasks = new UserRepairTask[3];
            for (int i = 0; i < userRepairTasks.Length; i++) {
                userRepairTasks[i] = new UserRepairTask {
                    RepairTask = new RepairTask { Id = 1, Status = Status.WorkedOn },
                    IsFinished = false,
                    Expert = technicians[i]
                };
            }
            this.dbContext.UsersRepairsTasks.AddRange(userRepairTasks);
            this.dbContext.SaveChanges();
            foreach (User technician in technicians) {
                await this.RepairTaskService.RemoveTechnicianFromRepairTaskAsync(technician.UserName, 1);
            }
            Assert.Equal(Status.Pending, userRepairTasks[0].RepairTask.Status);
        }

        [Fact]
        public async Task AllRepairTaskFilteredByCustomerAreReturned() {
            User customer = new User { UserName = "test" };
            await this.UserManager.CreateAsync(customer);
            await this.RoleManager.CreateAsync(new AppRole(StringConstants.CorporateCustomerUserRole));
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CorporateCustomerUserRole);

            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customer
                };
            }
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length, this.RepairTaskService.GetAllPerCustomer(customer.UserName).ToArray().Length);
        }

        [Fact]
        public async Task NotAllRepairTaskFilteredByCustomerAreReturnedBecauseNotAllRepairTasksAreForTheCustomer() {
            User[] customers = {
                new User { UserName = "test" },
                new User {UserName = "test2"}
            };
            foreach(User customer in customers) {
                await this.UserManager.CreateAsync(customer);
                await this.RoleManager.CreateAsync(new AppRole(StringConstants.CorporateCustomerUserRole));
                await this.UserManager.AddToRoleAsync(customer, StringConstants.CorporateCustomerUserRole);
            }
            RepairTask[] repairTasks = new RepairTask[3];
            for (int i = 0; i < repairTasks.Length; i++) {
                repairTasks[i] = new RepairTask {
                    User = customers[0]
                };
            }
            repairTasks[0].User = customers[1];
            this.dbContext.RepairTasks.AddRange(repairTasks);
            this.dbContext.SaveChanges();
            Assert.Equal(repairTasks.Length - 1, this.RepairTaskService.GetAllPerCustomer(customers[0].UserName).ToArray().Length);
        }

        [Fact]
        public async Task CannotLeaveARepairTaskWithNoParts() {
            RepairTask repairTask = new RepairTask {
                Id = 1,
                PartsRequired = new Part[] {
                    new Part{
                    Type = PartType.CarBody,
                    Quantity = 10
                    }
                }
            };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();
            RepairTaskEditInputModel inputModel = new RepairTaskEditInputModel {
                IsCarBodyPart = true,
                CarBodyPartAmount = 0,
                Id = 1
            };
            Assert.False(await this.RepairTaskService.TryUpdateRepairTaskAsync(inputModel));
        }

        [Fact]
        public async Task CanUpdateARepairTaskRequiredPartsQuantity() {
            RepairTask repairTask = new RepairTask {
                Id = 1,
                PartsRequired = new Part[] {
                    new Part{
                    Type = PartType.CarBody,
                    Quantity = 10
                    }
                }
            };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();
            RepairTaskEditInputModel inputModel = new RepairTaskEditInputModel {
                IsCarBodyPart = true,
                CarBodyPartAmount = 5,
                Id = 1
            };
            await this.RepairTaskService.TryUpdateRepairTaskAsync(inputModel);
            Assert.Equal(inputModel.CarBodyPartAmount, repairTask.PartsRequired.First().Quantity);
        }

        [Fact]
        public async Task CanUpdateAllRepairTaskPartsRequired() {
            RepairTask repairTask = new RepairTask {
                Id = 1,
                PartsRequired = new Part[] {
                    new Part{
                        Type = PartType.CarBody,
                        Quantity = 10
                    },
                    new Part {
                        Type  = PartType.Interior,
                        Quantity = 0
                    },
                    new Part {
                        Type  = PartType.Electronic,
                        Quantity = 0
                    },
                    new Part {
                        Type  = PartType.Chassis,
                        Quantity = 0
                    },
                }
            };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();
            RepairTaskEditInputModel inputModel = new RepairTaskEditInputModel {
                IsCarBodyPart = true,
                CarBodyPartAmount = 5,
                IsChassisPart = true,
                ChassisPartAmount = 2,
                IsElectronicPart = true,
                ElectronicPartAmount = 3,
                IsInteriorPart = true,
                InteriorPartAmount = 1,
                Id = 1
            };
            await this.RepairTaskService.TryUpdateRepairTaskAsync(inputModel);
            Assert.Equal(inputModel.CarBodyPartAmount, repairTask.PartsRequired.First(x => x.Type == PartType.CarBody).Quantity);
            Assert.Equal(inputModel.ChassisPartAmount, repairTask.PartsRequired.First(x => x.Type == PartType.Chassis).Quantity);
            Assert.Equal(inputModel.ElectronicPartAmount, repairTask.PartsRequired.First(x => x.Type == PartType.Electronic).Quantity);
            Assert.Equal(inputModel.InteriorPartAmount, repairTask.PartsRequired.First(x => x.Type == PartType.Interior).Quantity);
        }

        [Fact]
        public void CanDetectThatARepairTaskOrderIsNotChanged() {
            RepairTask repairTask = new RepairTask {
                Id = 1,
                PartsRequired = new Part[] {
                    new Part { Type = PartType.CarBody, Quantity = 10}
                }
            };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();

            RepairTaskEditInputModel inputModel = new RepairTaskEditInputModel {
                Id = 1,
                IsCarBodyPart = true,
                CarBodyPartAmount = 10
            };
            Assert.False(this.RepairTaskService.RepairTaskIsChanged(inputModel));
        }

        [Fact]
        public void CanDetectThatARepairTaskOrderIsChanged() {
            RepairTask repairTask = new RepairTask {
                Id = 1,
                PartsRequired = new Part[] {
                    new Part { Type = PartType.CarBody, Quantity = 10}
                }
            };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();

            RepairTaskEditInputModel inputModel = new RepairTaskEditInputModel {
                Id = 1,
                IsCarBodyPart = true,
                CarBodyPartAmount = 5
            };
            Assert.True(this.RepairTaskService.RepairTaskIsChanged(inputModel));
        }

        [Fact]
        public async Task CanDeleteARepairTask() {
            RepairTask repairTask = new RepairTask { Id = 1 };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();
            Assert.NotEmpty(this.dbContext.RepairTasks);
            await this.RepairTaskService.DeleteRepairTaskAsync(1);
            Assert.Empty(this.dbContext.RepairTasks);
        }
    }
}
