using Microsoft.AspNetCore.Identity;
using Project.Models.Entities;
using Project.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UserService(UserManager<User> userManager, RoleManager<AppRole> roleManager, SignInManager<User> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
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

        public async Task<bool> LogoutUserAsync() {
            await this.signInManager.SignOutAsync();
            return true;
        }
    }
}
