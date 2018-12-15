using Microsoft.AspNetCore.Identity;
using Project.Models.Entities;
using Project.Services.Contracts;
using System.Threading.Tasks;

namespace Project.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly UserManager<User> userManager;

        public RoleService(RoleManager<AppRole> roleManager, UserManager<User> userManager) {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<bool> AddUserToRoleAsync(User user, string role) {
            bool roleExists = await this.roleManager.RoleExistsAsync(role);
            if (!roleExists) {
                await this.roleManager.CreateAsync(new AppRole(role));
            }
            IdentityResult result = await this.userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }
    }
}
