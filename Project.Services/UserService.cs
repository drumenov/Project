using Microsoft.AspNetCore.Identity;
using Project.Data;
using Project.Models.Entities;
using Project.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly SignInManager<User> signInManager;
        private readonly ApplicationDbContext dbContext;

        public UserService(UserManager<User> userManager, 
            RoleManager<AppRole> roleManager, 
            SignInManager<User> signInManager,
            ApplicationDbContext dbContext) {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
        }

        public async Task<bool> RegisterUserAsync(User user, string password) {
            IdentityResult result = await this.userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        public async Task<bool> LoginUserAsync(string username, string password) {
            SignInResult result = await this.signInManager.PasswordSignInAsync(username, password, false, false);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetAllRolesThatUserBelongsToAsync(User user) {
            IList<string> rolesThatUserBelongTo = await this.userManager.GetRolesAsync(user);
            return rolesThatUserBelongTo;
        }

        public async Task LogoutUserAsync() {
            await this.signInManager.SignOutAsync();
        }

        public async Task<IQueryable<User>> GetAllUsersWithAGivenRoleAsync(string roleName) {
            AppRole currentRole = await this.roleManager.FindByNameAsync(roleName);
            var allUserIdsForAGivenRole =this.dbContext.UserRoles.Where(r => r.RoleId.Equals(currentRole.Id, StringComparison.Ordinal)).Select(res => res.UserId).ToArray();
            var allUsersForAGivenRole = this.dbContext.Users.Where(u => allUserIdsForAGivenRole.Contains(u.Id));
            return allUsersForAGivenRole;
        }

        public async Task<bool> AddUserToRoleAsync(User user, string roleName) {
            if (!await this.roleManager.RoleExistsAsync(roleName)) {
                await this.CreateRole(roleName);
            }
            IdentityResult result = await this.userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        private async Task CreateRole(string roleName) {
            IdentityResult result = await this.roleManager.CreateAsync(new AppRole { Name = roleName });
            if (!result.Succeeded) {
                throw new ApplicationException();
            }
        }
    }
}
