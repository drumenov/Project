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
    public class ReceiptService : IReceiptService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<User> userManager;

        public ReceiptService(ApplicationDbContext dbContext,
            UserManager<User> userManager) {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task GenerateReceiptAsync(ICollection<User> techniciansHavingWrokedOnRepairTask, string customerId, RepairTask repairTask) {
            decimal totalPrice = 0.0m;
            foreach (User technician in techniciansHavingWrokedOnRepairTask) {
                if (await this.userManager.IsInRoleAsync(technician, StringConstants.NoviceTechnicianUserRole)) {
                    totalPrice += IntegerConstants.NoviceTechnicianPriceRate;
                }
                else if (await this.userManager.IsInRoleAsync(technician, StringConstants.AverageTechnicianUserRole)) {
                    totalPrice += IntegerConstants.AverageTechnicianPriceRate;
                }
                else if (await this.userManager.IsInRoleAsync(technician, StringConstants.AdvancedTechnicianUserRole)) {
                    totalPrice += IntegerConstants.AdvancedTechnicianPriceRate;
                }
                else if (await this.userManager.IsInRoleAsync(technician, StringConstants.ExpertTechnicianUserRole)) {
                    totalPrice += IntegerConstants.ExpertTechnicianPriceRate;
                }
                else {
                    throw new ApplicationException();
                }
            }
            User customer = await this.userManager.FindByIdAsync(customerId);
            bool isCorporateCustomer = await this.userManager.IsInRoleAsync(customer, StringConstants.CorporateCustomerUserRole);
            if (isCorporateCustomer) {
                totalPrice *= IntegerConstants.CorporateClientPriceReduction;
            }
            Receipt receipt = new Receipt {
                UserId = customerId,
                TotalPrice = totalPrice
            };
            await this.dbContext.Receipts.AddAsync(receipt);

            if (await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
            repairTask.ReceiptId = receipt.Id;
            List<ExpertReceipt> expertsReceipts = new List<ExpertReceipt>();

            foreach (User technician in techniciansHavingWrokedOnRepairTask) {
                expertsReceipts.Add(new ExpertReceipt {
                    UserId = technician.Id,
                    ReceiptId = receipt.Id
                });
            }
            await this.dbContext
                .ExpertsReceipts
                .AddRangeAsync(expertsReceipts);
            if (await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
        }

        public decimal GetTotalRevenuePerCustomer(string customerName) {
            string customerId = this.userManager.FindByNameAsync(customerName).GetAwaiter().GetResult().Id;
            decimal totalRevenue = this.dbContext
                .Receipts
                .Where(receipt => receipt.UserId == customerId)
                .Sum(filteredReceipts => filteredReceipts.TotalPrice);
            return totalRevenue;
        }

        public IQueryable<Receipt> GetAll() {
            return this.dbContext
                .Receipts;
        }

        public IQueryable<Receipt> GetById(int id) {
            return this.dbContext.Receipts.Where(receipt => receipt.Id == id);
        }
    }
}
