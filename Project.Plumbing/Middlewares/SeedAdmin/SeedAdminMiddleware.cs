using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Project.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Plumbing.Middlewares.SeedAdmin
{
    public class SeedAdminMiddleware
    {
        private readonly RequestDelegate next;

        public SeedAdminMiddleware(RequestDelegate next) {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, RoleManager<AppRole> roleManager, UserManager<User> userManager) {
            if (!userManager.Users.Any()) {
                await roleManager.CreateAsync(new AppRole("Admin"));
                User admin = new User { UserName = "admin" };
                await userManager.CreateAsync(admin, "123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
            await this.next(context);
        }
    }
}
