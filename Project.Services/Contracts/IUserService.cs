using Project.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(User user, string password);

        Task<bool> LoginUserAsync(string username, string password);

        Task<IList<string>> GetAllRolesThatUserBelongsToAsync(User user);

        Task LogoutUserAsync();
    }
}
