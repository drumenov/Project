﻿using Microsoft.AspNetCore.Identity;
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

        public TechnicianService(ApplicationDbContext dbContext,
            UserManager<User> userManager) {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task AddTechniciansToRepairTaskAsync(ICollection<string> availableTechniciansName, int taskId) {
            foreach (string technicianName in availableTechniciansName) {
                User user = await this.userManager.FindByNameAsync(technicianName);
                this.dbContext
                    .UsersRepairsTasks
                    .Add(new UserRepairTask {
                        RepairTaskId = taskId,
                        UserId = user.Id
                    });
            }
            if(await this.dbContext.SaveChangesAsync() == 0) {
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

        
    }
}
