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
                Part part = this.GetPart(PartType.CarBody);
                part.Quantity += createPartOrderInputModel.CarBodyPartAmount;
                order.OrderedParts.Add(part);
            }
            if (createPartOrderInputModel.IsChassisPart) {
                Part part = this.GetPart(PartType.Chassis);
                part.Quantity += createPartOrderInputModel.ChassisPartAmount;
                order.OrderedParts.Add(part);
            }
            if (createPartOrderInputModel.IsElectronicPart) {
                Part part = this.GetPart(PartType.Electronic);
                part.Quantity += createPartOrderInputModel.ElectronicPartAmount;
                order.OrderedParts.Add(part);
            }
            if (createPartOrderInputModel.IsInteriorPart) {
                Part part = this.GetPart(PartType.Interior);
                part.Quantity += createPartOrderInputModel.InteriorPartAmount;
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

        private Part GetPart(PartType partType) {
            return this.dbContext
                .Parts
                .SingleOrDefault(p => p.Type == partType);
        }
    }
}
