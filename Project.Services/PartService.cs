using Project.Data;
using Project.Models.Entities;
using Project.Models.Enums;
using Project.Models.InputModels.Administration;
using Project.Services.Contracts;
using System;
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

        //private void IncreasePartTypeAvailableCount(PartType partType, int partAmountOrdered) {
        //    Part part = this.GetPart(partType);
        //    part.Quantity += partAmountOrdered;
        //}

        //private Part GetPart(PartType partType) {
        //    return this.dbContext
        //        .Parts
        //        .SingleOrDefault(p => p.Type == partType);
        //}
    }
}
