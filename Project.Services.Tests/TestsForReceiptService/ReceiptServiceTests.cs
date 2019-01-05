using Project.Common.Constants;
using Project.Models.Entities;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Project.Services.Tests.TestsForReceiptService
{
    public class ReceiptServiceTests : Base
    {
        [Fact]
        public async Task CanCreateReceipt() {
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerControllerName });
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            User[] technicians = {
                new User {UserName = "test"},
                new User {UserName = "test2"},
                new User {UserName = "test3"}
            };
            foreach(User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
                await this.UserManager.AddToRoleAsync(technician, StringConstants.NoviceTechnicianUserRole);
            }
            User customer = new User { UserName = "test4" };
            await this.UserManager.CreateAsync(customer);
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);

            RepairTask repairTask = new RepairTask {
                Id = 1
            };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();

            Assert.Empty(this.dbContext.Receipts);
            string customerId = this.UserManager.FindByNameAsync(customer.UserName).GetAwaiter().GetResult().Id;
            await this.ReceiptService.GenerateReceiptAsync(technicians, customerId, repairTask);
            Assert.NotEmpty(this.dbContext.Receipts);
        }

        [Fact]
        public async Task CalculationIsProperForNoviceTechnicianAndNonCorporateClient() {
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerControllerName });
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            User[] technicians = {
                new User {UserName = "test"},
                new User {UserName = "test2"},
                new User {UserName = "test3"}
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
                await this.UserManager.AddToRoleAsync(technician, StringConstants.NoviceTechnicianUserRole);
            }
            User customer = new User { UserName = "test4" };
            await this.UserManager.CreateAsync(customer);
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);

            RepairTask repairTask = new RepairTask {
                Id = 1
            };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();

            string customerId = this.UserManager.FindByNameAsync(customer.UserName).GetAwaiter().GetResult().Id;
            await this.ReceiptService.GenerateReceiptAsync(technicians, customerId, repairTask);
            Receipt receipt = this.dbContext.Receipts.First();
            Assert.Equal(IntegerConstants.NoviceTechnicianPriceRate * technicians.Length, receipt.TotalPrice);
        }

        [Fact]
        public async Task CalculationIsProperForAverageTechnicianAndNonCorporateClient() {
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerControllerName });
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.AverageTechnicianUserRole });
            User[] technicians = {
                new User {UserName = "test"},
                new User {UserName = "test2"},
                new User {UserName = "test3"}
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
                await this.UserManager.AddToRoleAsync(technician, StringConstants.AverageTechnicianUserRole);
            }
            User customer = new User { UserName = "test4" };
            await this.UserManager.CreateAsync(customer);
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);

            RepairTask repairTask = new RepairTask {
                Id = 1
            };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();

            string customerId = this.UserManager.FindByNameAsync(customer.UserName).GetAwaiter().GetResult().Id;
            await this.ReceiptService.GenerateReceiptAsync(technicians, customerId, repairTask);
            Receipt receipt = this.dbContext.Receipts.First();
            Assert.Equal(IntegerConstants.AverageTechnicianPriceRate * technicians.Length, receipt.TotalPrice);
        }

        [Fact]
        public async Task CalculationIsProperForAdvancedTechnicianAndNonCorporateClient() {
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerControllerName });
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.AdvancedTechnicianUserRole });
            User[] technicians = {
                new User {UserName = "test"},
                new User {UserName = "test2"},
                new User {UserName = "test3"}
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
                await this.UserManager.AddToRoleAsync(technician, StringConstants.AdvancedTechnicianUserRole);
            }
            User customer = new User { UserName = "test4" };
            await this.UserManager.CreateAsync(customer);
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);

            RepairTask repairTask = new RepairTask {
                Id = 1
            };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();

            string customerId = this.UserManager.FindByNameAsync(customer.UserName).GetAwaiter().GetResult().Id;
            await this.ReceiptService.GenerateReceiptAsync(technicians, customerId, repairTask);
            Receipt receipt = this.dbContext.Receipts.First();
            Assert.Equal(IntegerConstants.AdvancedTechnicianPriceRate * technicians.Length, receipt.TotalPrice);
        }

        [Fact]
        public async Task CalculationIsProperForExpertTechnicianAndNonCorporateClient() {
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CustomerControllerName });
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.ExpertTechnicianUserRole });
            User[] technicians = {
                new User {UserName = "test"},
                new User {UserName = "test2"},
                new User {UserName = "test3"}
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
                await this.UserManager.AddToRoleAsync(technician, StringConstants.ExpertTechnicianUserRole);
            }
            User customer = new User { UserName = "test4" };
            await this.UserManager.CreateAsync(customer);
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CustomerUserRole);

            RepairTask repairTask = new RepairTask {
                Id = 1
            };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();

            string customerId = this.UserManager.FindByNameAsync(customer.UserName).GetAwaiter().GetResult().Id;
            await this.ReceiptService.GenerateReceiptAsync(technicians, customerId, repairTask);
            Receipt receipt = this.dbContext.Receipts.First();
            Assert.Equal(IntegerConstants.ExpertTechnicianPriceRate * technicians.Length, receipt.TotalPrice);
        }

        [Fact]
        public async Task CalculationIsProperForNoviceTechnicianAndCorporateClient() {
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CorporateCustomerUserRole });
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.NoviceTechnicianUserRole });
            User[] technicians = {
                new User {UserName = "test"},
                new User {UserName = "test2"},
                new User {UserName = "test3"}
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
                await this.UserManager.AddToRoleAsync(technician, StringConstants.NoviceTechnicianUserRole);
            }
            User customer = new User { UserName = "test4" };
            await this.UserManager.CreateAsync(customer);
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CorporateCustomerUserRole);

            RepairTask repairTask = new RepairTask {
                Id = 1
            };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();

            string customerId = this.UserManager.FindByNameAsync(customer.UserName).GetAwaiter().GetResult().Id;
            await this.ReceiptService.GenerateReceiptAsync(technicians, customerId, repairTask);
            Receipt receipt = this.dbContext.Receipts.First();
            Assert.Equal(IntegerConstants.NoviceTechnicianPriceRate * technicians.Length * IntegerConstants.CorporateClientPriceReduction, receipt.TotalPrice);
        }

        [Fact]
        public async Task CalculationIsProperForAverageTechnicianAndCorporateClient() {
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CorporateCustomerUserRole });
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.AverageTechnicianUserRole });
            User[] technicians = {
                new User {UserName = "test"},
                new User {UserName = "test2"},
                new User {UserName = "test3"}
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
                await this.UserManager.AddToRoleAsync(technician, StringConstants.AverageTechnicianUserRole);
            }
            User customer = new User { UserName = "test4" };
            await this.UserManager.CreateAsync(customer);
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CorporateCustomerUserRole);

            RepairTask repairTask = new RepairTask {
                Id = 1
            };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();

            string customerId = this.UserManager.FindByNameAsync(customer.UserName).GetAwaiter().GetResult().Id;
            await this.ReceiptService.GenerateReceiptAsync(technicians, customerId, repairTask);
            Receipt receipt = this.dbContext.Receipts.First();
            Assert.Equal(IntegerConstants.AverageTechnicianPriceRate * technicians.Length * IntegerConstants.CorporateClientPriceReduction, receipt.TotalPrice);
        }

        [Fact]
        public async Task CalculationIsProperForAdvancedTechnicianAndCorporateClient() {
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CorporateCustomerUserRole });
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.AdvancedTechnicianUserRole });
            User[] technicians = {
                new User {UserName = "test"},
                new User {UserName = "test2"},
                new User {UserName = "test3"}
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
                await this.UserManager.AddToRoleAsync(technician, StringConstants.AdvancedTechnicianUserRole);
            }
            User customer = new User { UserName = "test4" };
            await this.UserManager.CreateAsync(customer);
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CorporateCustomerUserRole);

            RepairTask repairTask = new RepairTask {
                Id = 1
            };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();

            string customerId = this.UserManager.FindByNameAsync(customer.UserName).GetAwaiter().GetResult().Id;
            await this.ReceiptService.GenerateReceiptAsync(technicians, customerId, repairTask);
            Receipt receipt = this.dbContext.Receipts.First();
            Assert.Equal(IntegerConstants.AdvancedTechnicianPriceRate * technicians.Length * IntegerConstants.CorporateClientPriceReduction, receipt.TotalPrice);
        }

        [Fact]
        public async Task CalculationIsProperForExpertTechnicianAndCorporateClient() {
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.CorporateCustomerUserRole });
            await this.RoleManager.CreateAsync(new AppRole { Name = StringConstants.ExpertTechnicianUserRole });
            User[] technicians = {
                new User {UserName = "test"},
                new User {UserName = "test2"},
                new User {UserName = "test3"}
            };
            foreach (User technician in technicians) {
                await this.UserManager.CreateAsync(technician);
                await this.UserManager.AddToRoleAsync(technician, StringConstants.ExpertTechnicianUserRole);
            }
            User customer = new User { UserName = "test4" };
            await this.UserManager.CreateAsync(customer);
            await this.UserManager.AddToRoleAsync(customer, StringConstants.CorporateCustomerUserRole);

            RepairTask repairTask = new RepairTask {
                Id = 1
            };
            this.dbContext.RepairTasks.Add(repairTask);
            this.dbContext.SaveChanges();

            string customerId = this.UserManager.FindByNameAsync(customer.UserName).GetAwaiter().GetResult().Id;
            await this.ReceiptService.GenerateReceiptAsync(technicians, customerId, repairTask);
            Receipt receipt = this.dbContext.Receipts.First();
            Assert.Equal(IntegerConstants.ExpertTechnicianPriceRate * technicians.Length * IntegerConstants.CorporateClientPriceReduction, receipt.TotalPrice);
        }

        [Fact]
        public void CanReturnAllReceiptsFromDb() {
            Receipt[] receipts = {
                new Receipt(),
                new Receipt(),
                new Receipt()
            };
            this.dbContext.Receipts.AddRange(receipts);
            this.dbContext.SaveChanges();
            Assert.Equal(receipts.Length, this.ReceiptService.GetAll().ToArray().Length);
        }

        [Fact]
        public void CanReturnTheProperReceipt() {
            Receipt receipt = new Receipt { Id = 1 };
            this.dbContext.Receipts.Add(receipt);
            this.dbContext.SaveChanges();
            Assert.Same(receipt, this.ReceiptService.GetById(receipt.Id).First());
        }

        [Fact]
        public void CanReturnTheProperReceiptSecondTest() {
            Receipt[] receipts = {
                new Receipt{Id = 1},
                new Receipt{Id = 2}
            };
            this.dbContext.Receipts.AddRange(receipts);
            this.dbContext.SaveChanges();
            Assert.NotSame(receipts[0], this.ReceiptService.GetById(receipts[1].Id));
        }
    }
}
