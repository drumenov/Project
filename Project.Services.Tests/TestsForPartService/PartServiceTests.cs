using Project.Models.Entities;
using Project.Models.Enums;
using Project.Models.InputModels.Administration;
using Project.Models.ViewModels.Administration;
using System.Threading.Tasks;
using Xunit;

namespace Project.Services.Tests.TestsForPartService
{
    public class PartServiceTests : Base
    {
        [Fact]
        public async Task CanOrderPart() {
            CreatePartOrderInputModel inputModel = new CreatePartOrderInputModel {
                IsCarBodyPart = true,
                CarBodyPartAmount = 5
            };
            User user = new User { UserName = "test" };
            Assert.Equal(1, await this.PartService.OrderPartsAsync(inputModel, user));
            Assert.NotEmpty(this.dbContext.Parts);
        }

        [Fact]
        public void ReturnsTrueIfPartTypeExistsInDb() {
            Part part = new Part {
                Type = PartType.CarBody
            };
            this.dbContext.Parts.Add(part);
            this.dbContext.SaveChanges();
            Assert.True(this.PartService.PartTypeExists(PartType.CarBody));
        }

        [Fact]
        public void ReturnsFalseIfPartTypeDoesNotExistInDb() {
            Part part = new Part {
                Type = PartType.CarBody
            };
            this.dbContext.Parts.Add(part);
            this.dbContext.SaveChanges();
            Assert.False(this.PartService.PartTypeExists(PartType.Interior));
        }

        [Fact]
        public void ReturnsTrueIfAllRequiredPartsAreAvailable() {
            Order order = new Order {
                OrderedParts = new Part[] {
                    new Part { Type = PartType.CarBody, Quantity = 10 },
                    new Part { Type = PartType.Chassis, Quantity = 10 },
                    new Part { Type = PartType.Electronic, Quantity = 10 },
                    new Part { Type = PartType.Interior, Quantity = 10 }
                }
            };
            this.dbContext.Orders.Add(order);
            this.dbContext.SaveChanges();
            RepairTaskSimpleInfoViewModel repairTaskSimpleInfoViewModel = new RepairTaskSimpleInfoViewModel {
                PartsRequired = new Part[] {
                    new Part { Type = PartType.CarBody, Quantity = 5},
                    new Part { Type = PartType.Chassis, Quantity = 5},
                    new Part { Type = PartType.Electronic, Quantity = 5},
                    new Part { Type = PartType.Interior, Quantity = 5}
                }
            };
            this.PartService.AllPartsForRepairTaskAreAvailable(repairTaskSimpleInfoViewModel);
            Assert.True(repairTaskSimpleInfoViewModel.CanBeAssigned);
        }

        [Fact]
        public void ReturnsFalseIfNotAllRequiredPartsAreAvailable() {
            Part[] parts = {
                new Part { Type = PartType.CarBody, Quantity = 10 },
                new Part { Type = PartType.Chassis, Quantity = 10 },
                new Part { Type = PartType.Electronic, Quantity = 10 },
                new Part { Type = PartType.Interior, Quantity = 10 }
            };
            this.dbContext.Parts.AddRange(parts);
            this.dbContext.SaveChanges();
            RepairTaskSimpleInfoViewModel repairTaskSimpleInfoViewModel = new RepairTaskSimpleInfoViewModel {
                PartsRequired = new Part[] {
                    new Part { Type = PartType.CarBody, Quantity = 11},
                    new Part { Type = PartType.Chassis, Quantity = 5},
                    new Part { Type = PartType.Electronic, Quantity = 5},
                    new Part { Type = PartType.Interior, Quantity = 5}
                }
            };
            this.PartService.AllPartsForRepairTaskAreAvailable(repairTaskSimpleInfoViewModel);
            Assert.False(repairTaskSimpleInfoViewModel.CanBeAssigned);
        }
    }
}
