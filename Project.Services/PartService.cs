using Project.Common.Constants;
using Project.Data;
using Project.Models.Entities;
using Project.Models.Enums;
using Project.Models.InputModels.Administration;
using Project.Models.ViewModels.Administration;
using Project.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Services
{
    public class PartService : IPartService
    {
        private readonly ApplicationDbContext dbContext;

        public PartService(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public async Task<int> OrderPartsAsync(CreatePartOrderInputModel createPartOrderInputModel, User admin) {
            Order order = new Order {
                User = admin
            };
            if (createPartOrderInputModel.IsCarBodyPart) {
                Part part = new Part {
                    Type = PartType.CarBody,
                    Quantity = createPartOrderInputModel.CarBodyPartAmount
                };
                order.OrderedParts.Add(part);
            }
            if (createPartOrderInputModel.IsChassisPart) {
                Part part = new Part {
                    Type = PartType.Chassis,
                    Quantity = createPartOrderInputModel.ChassisPartAmount
                };
                order.OrderedParts.Add(part);
            }
            if (createPartOrderInputModel.IsElectronicPart) {
                Part part = new Part {
                    Type = PartType.Electronic,
                    Quantity = createPartOrderInputModel.ElectronicPartAmount
                };
                order.OrderedParts.Add(part);
            }
            if (createPartOrderInputModel.IsInteriorPart) {
                Part part = new Part {
                    Type = PartType.Interior,
                    Quantity = createPartOrderInputModel.InteriorPartAmount
                };
                order.OrderedParts.Add(part);
            }
            await this.dbContext
                .Orders
                .AddAsync(order);
            if (await this.dbContext.SaveChangesAsync() <= 0) {
                throw new ApplicationException();
            }
            else {
                return order.Id;
            }
        }
        
        public bool PartTypeExists(PartType partType) {
            return this.dbContext
                .Parts
                .Any(part => part.Type == partType);
        }

        public void AllPartsForRepairTaskAreAvailable(RepairTaskSimpleInfoViewModel repairTaskSimpleInfoViewModel) {

            var t = this.dbContext
                .RepairTask
                .Where(rt => rt.Status != Status.Pending)
                .SelectMany(x => x.PartsRequired)
                .Where(r => r.Type == PartType.CarBody)
                .Sum(u => u.Quantity);

            bool repairTaskCanBeAssgned = true;
            foreach(Part requiredPart in repairTaskSimpleInfoViewModel.PartsRequired) {
                int orderedPartsOfType = this.dbContext
                                                    .Parts
                                                    .Where(part => requiredPart.Type == part.Type)
                                                    .Sum(filteredParts => filteredParts.Quantity);
                int usedPartsOfType = this.dbContext
                                            .RepairTask
                                            .Where(repairTask => repairTask.Status != Status.Pending)
                                            .SelectMany(filteredRepairTasks => filteredRepairTasks.PartsRequired)
                                            .Where(part => part.Type == PartType.CarBody)
                                            .Sum(filteredParts => filteredParts.Quantity);

                int totalAvailablePartsOfType = Math.Abs(orderedPartsOfType - usedPartsOfType);
                if (requiredPart.Quantity > orderedPartsOfType - usedPartsOfType) {
                    repairTaskCanBeAssgned = false;
                    repairTaskSimpleInfoViewModel
                        .PartsMissingMeesage
                        .Add(String.Format(StringConstants.NotEnoughPartsAvailable,requiredPart.Type, totalAvailablePartsOfType));
                }
                repairTaskSimpleInfoViewModel.CanBeAssigned = repairTaskCanBeAssgned;
            }
        }
    }
}
