using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project.Data;
using Project.Models.Entities;
using Project.Services.Contracts;
using System;

namespace Project.Services.Tests
{
    public class Base { 

        public Base() {

            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(opt => {
                opt.UseInMemoryDatabase("DbForTestingPurposes");
            });
            services.AddIdentity<User, AppRole>(identityOptions => {
                identityOptions.Password.RequireDigit = false;
                identityOptions.Password.RequiredLength = 0;
                identityOptions.Password.RequiredUniqueChars = 0;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<IPartService, PartService>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddScoped<IRepairTaskService, RepairTaskService>();


            IServiceProvider provider = services.BuildServiceProvider();

            this.UserService = new UserService(provider.GetService<UserManager<User>>(), 
                provider.GetService<RoleManager<AppRole>>(), 
                provider.GetService<SignInManager<User>>(), 
                provider.GetService<ApplicationDbContext>());

            this.TechnicianService = new TechnicianService(provider.GetService<ApplicationDbContext>(),
                                                            provider.GetService<UserManager<User>>(),
                                                            provider.GetService<IRepairTaskService>(),
                                                            provider.GetService<RoleManager<AppRole>>());

            this.dbContext = provider.GetService<ApplicationDbContext>();

            this.UserManager = provider.GetService<UserManager<User>>();

            this.RoleManager = provider.GetService<RoleManager<AppRole>>();
        }

        public ApplicationDbContext dbContext { get; set; }

        public UserManager<User> UserManager { get; }

        public RoleManager<AppRole> RoleManager { get; }

        public IUserService UserService { get; }

        public ITechnicianService TechnicianService { get; }
    }
}
