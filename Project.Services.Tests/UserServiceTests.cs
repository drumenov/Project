using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Project.Models.Entities;
using System.Threading.Tasks;
using Xunit;

namespace Project.Services.Tests
{
    public class UserServiceTests : Base
    {

        [Fact]
        public async Task UserRegisterSuccessfully() {
            User user = new User {
                UserName = "TestUser"
            };
            string password = "TestPassword";
            bool result = await this.UserService.RegisterUserAsync(user, password);
            Assert.True(result);
        }
    }
}
