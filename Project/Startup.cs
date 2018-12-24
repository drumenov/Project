using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Data;
using Project.Models.Entities;
using Project.Models.InputModels.Administration;
using Project.Models.ViewModels.Administration;
using Project.Models.ViewModels.Customer;
using Project.Plumbing.Middlewares.SeedAdmin;
using Project.Services;
using Project.Services.Contracts;
using System.Linq;

namespace Project
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {

            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection"));
                options.UseLazyLoadingProxies();
            });

            services.AddIdentity<User, AppRole>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 0;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.ConfigureApplicationCookie(options => {
                options.AccessDeniedPath = "/home/unauthorised-access";
                options.LoginPath = "/users/login";
            });


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPartService, PartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPartService, PartService>();
            services.AddScoped<IRepairTaskService, RepairTaskService>();
            services.AddScoped<ITechnicianService, TechnicianService>();
            services.AddScoped<IReceiptService, ReceiptService>();

            services.AddAutoMapper(config => {
                config.CreateMap<CreateAdministratorInputModel, User>();
                config.CreateMap<CreateCustomerInputModel, User>()
                    .ForMember(dest => dest.UserName, src => src.MapFrom(s => s.CustomerName));
                config.CreateMap<CreateTechnicianInputModel, User>()
                    .ForMember(dest => dest.UserName, src => src.MapFrom(s => s.Name));
                config.CreateMap<Order, OrderViewModel>()
                    .ForMember(dest => dest.Username, src => src.MapFrom(s => s.User.UserName));
                config.CreateMap<RepairTask, RepairTaskDetailsViewModel>()
                    .ForMember(dest => dest.Technicians, src => src.MapFrom(s => s.Technicians.SelectMany(t => t.Expert.UserName)));
                config.CreateMap<RepairTask, Models.ViewModels.Administration.RepairTaskSimpleInfoViewModel>()
                    .ForMember(dest => dest.Username, src => src.MapFrom(s => s.User.UserName));
            });
            services.AddAuthentication().AddCookie();
            services.AddMvc(options => {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/home/error");
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSeedAdminMiddleware();
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );

            });
        }
    }
}
