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
        private readonly IRepairTaskService repairTaskService;
        private readonly UserManager<User> userManager;

        public ReceiptService(ApplicationDbContext dbContext,
            IRepairTaskService repairTaskService,
            UserManager<User> userManager) {
            this.dbContext = dbContext;
            this.repairTaskService = repairTaskService;
            this.userManager = userManager;
        }

        public async Task GenerateReceiptAsync(int repairTaskId) {
            decimal totalPrice = 0.0m;
            User[] techniciansHavingWorkedOnTheRepairTask = this.repairTaskService
                                                                    .GetTechniciansHavingWorkedOnARepairTask(repairTaskId)
                                                                    .ToArray();
            foreach (User technician in techniciansHavingWorkedOnTheRepairTask) {
                if(await this.userManager.IsInRoleAsync(technician, StringConstants.NoviceTechnicianUserRole)){
                    totalPrice += 25m;
                } else if(await this.userManager.IsInRoleAsync(technician, StringConstants.AverageTechnicianUserRole)) {
                    totalPrice += 50m;
                } else if(await this.userManager.IsInRoleAsync(technician, StringConstants.AdvancedTechnicianUserRole)) {
                    totalPrice += 75m;
                } else if(await this.userManager.IsInRoleAsync(technician, StringConstants.ExpertTechnicianUserRole)) {
                    totalPrice += 100m;
                } else {
                    throw new ApplicationException();
                }
            }
            Receipt receipt = new Receipt {
                UserId = this.repairTaskService.GetById(repairTaskId).UserId,
                TotalPrice = totalPrice
            };

            await this.dbContext.Receipts.AddAsync(receipt);

            if(await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }

            List<ExpertReceipt> expertsReceipts = new List<ExpertReceipt>();

            foreach (User technician in techniciansHavingWorkedOnTheRepairTask) {
                expertsReceipts.Add(new ExpertReceipt {
                    UserId = technician.Id,
                    ReceiptId = receipt.Id
                });
            }
            await this.dbContext
                .ExpertsReceipts
                .AddRangeAsync(expertsReceipts);
            if(await this.dbContext.SaveChangesAsync() == 0) {
                throw new ApplicationException();
            }
        }
    }
}
