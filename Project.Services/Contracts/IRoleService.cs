using Project.Models.Entities;
using System.Threading.Tasks;

namespace Project.Services.Contracts
{
    public interface IRoleService
    {
        Task<bool> AddUserToRoleAsync(User user, string role);
    }
}
