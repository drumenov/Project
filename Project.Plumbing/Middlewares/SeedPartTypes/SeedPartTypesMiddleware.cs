using Microsoft.AspNetCore.Http;
using Project.Data;
using Project.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Plumbing.Middlewares.SeedPartTypes
{
    public class SeedPartTypesMiddleware
    {
        private readonly RequestDelegate next;

        public SeedPartTypesMiddleware(RequestDelegate next) {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext) {
            if (dbContext.Parts.Count() == 0) {
                List<Part> partTypes = new List<Part> {
                new Part { Type = Models.Enums.PartType.Chassis },
                new Part { Type = Models.Enums.PartType.CarBody},
                new Part { Type = Models.Enums.PartType.Electronic},
                new Part { Type = Models.Enums.PartType.Interior}
                };
                await dbContext.Parts.AddRangeAsync(partTypes);
                await dbContext.SaveChangesAsync();
            }
            await this.next(context);
        }
    }
}
