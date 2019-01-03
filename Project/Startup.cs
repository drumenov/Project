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
using Project.Models.InputModels.Customer;
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
                options.AccessDeniedPath = "/home/unauthorised-access"; //TODO: Make proper routing
                options.LoginPath = "/users/login"; //TODO: Make proper routing
            });


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPartService, PartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPartService, PartService>();
            services.AddScoped<IRepairTaskService, RepairTaskService>();
            services.AddScoped<ITechnicianService, TechnicianService>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddScoped<IFeedbackService, FeedbackService>();

            services.AddAutoMapper(config => {
                config.CreateMap<CreateAdministratorInputModel, User>();
                config.CreateMap<CreateCustomerInputModel, User>()
                    .ForMember(dest => dest.UserName, src => src.MapFrom(s => s.CustomerName));
                config.CreateMap<CreateTechnicianInputModel, User>()
                    .ForMember(dest => dest.UserName, src => src.MapFrom(s => s.Name));
                config.CreateMap<Order, OrderViewModel>()
                    .ForMember(dest => dest.Username, src => src.MapFrom(s => s.User.UserName));
                config.CreateMap<RepairTask, Models.ViewModels.Customer.RepairTaskDetailsViewModel>()
                    .ForMember(dest => dest.Technicians, src => src.MapFrom(s => s.Technicians.Select(t => t.Expert.UserName)));
                config.CreateMap<RepairTask, RepairTaskSimpleInfoViewModel>()
                    .ForMember(dest => dest.Username, src => src.MapFrom(s => s.User.UserName));
                config.CreateMap<RepairTask, Models.ViewModels.Administration.RepairTaskDetailsViewModel>()
                    .ForMember(dest => dest.Username, src => src.MapFrom(s => s.User.UserName))
                    .ForMember(dest => dest.TechniciansWorkingOnRepairTask, src => src.MapFrom(s => s.Technicians.Select(t => t.Expert).ToArray()))
                    .ForMember(dest => dest.PartsRequired, src => src.MapFrom(s => s.PartsRequired));
                config.CreateMap<RepairTask, Models.ViewModels.Technician.RepairTaskDetailsViewModel>()
                    .ForMember(dest => dest.Username, src => src.MapFrom(s => s.User.UserName))
                    .ForMember(dest => dest.TechniciansWorkingOnRepairTask, src => src.MapFrom(s => s.Technicians.Select(t => t.Expert).ToArray()))
                    .ForMember(dest => dest.PartsRequired, src => src.MapFrom(s => s.PartsRequired));
                config.CreateMap<RepairTask, Models.ViewModels.Administration.RepairTaskViewModel>()
                    .ForMember(dest => dest.Username, src => src.MapFrom(s => s.User.UserName))
                    .ForMember(dest => dest.CountOfTechniciansCurrentlyWorkingOnTheRepairTask, src => src.MapFrom(s => s.Technicians.Where(t => t.IsFinished == false).Count()))
                    .ForMember(dest => dest.CountOfTechniciansHavingFinishedWorkingOnTheRepairTask, src => src.MapFrom(s => s.Technicians.Where(t => t.IsFinished).Count()));
                config.CreateMap<RepairTask, Models.ViewModels.Administration.RepairTaskReceiptViewModel>()
                    .ForMember(dest => dest.ReceiptId, src => src.MapFrom(s => s.ReceiptId))
                    .ForMember(dest => dest.TotalRevenue, src => src.MapFrom(s => s.Receipt.TotalPrice))
                    .ForMember(dest => dest.CustomerName, src => src.MapFrom(s => s.User.UserName))
                    .ForMember(dest => dest.NamesOfTechniciansHavingWorkedOnTheRepairTask, src => src.MapFrom(s => s.Technicians.Select(t => t.Expert.UserName)));
                config.CreateMap<User, TechnicianDetailsViewModel>()
                    .ForMember(dest => dest.Username, src => src.MapFrom(s => s.UserName))
                    .ForMember(dest => dest.CountOfFinishedRepairTasks, src => src.MapFrom(s => s.RepairTasks.Where(repairTask => repairTask.IsFinished).Count()))
                    .ForMember(dest => dest.CountOfWorkedOnRepairTasks, src => src.MapFrom(s => s.RepairTasks.Where(repairTak => repairTak.IsFinished == false).Count()));
                config.CreateMap<Receipt, ReceiptViewModel>()
                    .ForMember(dest => dest.Customer, src => src.MapFrom(s => s.User.UserName))
                    .ForMember(dest => dest.Id, src => src.MapFrom(s => s.Id))
                    .ForMember(dest => dest.TotalRevenue, src => src.MapFrom(s => s.TotalPrice));
                config.CreateMap<RepairTask, RepairTaskInformationViewModel>()
                    .ForMember(dest => dest.Id, src => src.MapFrom(s => s.Id))
                    .ForMember(dest => dest.Status, src => src.MapFrom(s => s.Status))
                    .ForMember(dest => dest.Cost, src => src.MapFrom(s => s.Receipt.TotalPrice));
                config.CreateMap<RepairTask, Models.ViewModels.Customer.RepairTaskReceiptViewModel>()
                    .ForMember(dest => dest.ReceiptId, src => src.MapFrom(s => s.ReceiptId))
                    .ForMember(dest => dest.TotalRevenue, src => src.MapFrom(s => s.Receipt.TotalPrice))
                    .ForMember(dest => dest.CustomerName, src => src.MapFrom(s => s.User.UserName))
                    .ForMember(dest => dest.NamesOfTechniciansHavingWorkedOnTheRepairTask, src => src.MapFrom(s => s.Technicians.Select(t => t.Expert.UserName)));
                config.CreateMap<RepairTask, RepairTaskEditInputModel>()
                    .ForMember(dest => dest.Id, src => src.MapFrom(s => s.Id))
                    .ForMember(dest => dest.IsInteriorPart, src => src.MapFrom(s => s.PartsRequired
                                                                                        .Where(partRequired => partRequired.Type == Models.Enums.PartType.Interior)
                                                                                        .Any(filteredPartRequired => filteredPartRequired.Quantity > 0)))
                    .ForMember(dest => dest.InteriorPartAmount, src => src.MapFrom(s => s.PartsRequired
                                                                                            .Where(partRequired => partRequired.Type == Models.Enums.PartType.Interior)
                                                                                            .FirstOrDefault()
                                                                                            .Quantity))
                    .ForMember(dest => dest.IsElectronicPart, src => src.MapFrom(s => s.PartsRequired
                                                                                       .Where(partRequired => partRequired.Type == Models.Enums.PartType.Electronic)
                                                                                        .Any(filteredPartRequired => filteredPartRequired.Quantity > 0)))
                    .ForMember(dest => dest.ElectronicPartAmount, src => src.MapFrom(s => s.PartsRequired
                                                                                            .Where(partRequired => partRequired.Type == Models.Enums.PartType.Electronic)
                                                                                            .FirstOrDefault()
                                                                                            .Quantity))
                    .ForMember(dest => dest.IsChassisPart, src => src.MapFrom(s => s.PartsRequired
                                                                                            .Where(partRequired => partRequired.Type == Models.Enums.PartType.Chassis)
                                                                                            .Any(filteredPartRequired => filteredPartRequired.Quantity > 0)))
                    .ForMember(dest => dest.ChassisPartAmount, src => src.MapFrom(s => s.PartsRequired
                                                                                        .Where(partRequired => partRequired.Type == Models.Enums.PartType.Chassis)
                                                                                        .FirstOrDefault()
                                                                                        .Quantity))
                    .ForMember(dest => dest.IsCarBodyPart, src => src.MapFrom(s => s.PartsRequired
                                                                                            .Where(partRequired => partRequired.Type == Models.Enums.PartType.CarBody)
                                                                                            .Any(filteredPartRequired => filteredPartRequired.Quantity > 0)))
                    .ForMember(dest => dest.CarBodyPartAmount, src => src.MapFrom(s => s.PartsRequired
                                                                                        .Where(partRequired => partRequired.Type == Models.Enums.PartType.CarBody)
                                                                                        .FirstOrDefault()
                                                                                        .Quantity));
                config.CreateMap<RepairTask, Models.ViewModels.Customer.RepairTaskViewModel>()
                    .ForMember(dest => dest.Id, src => src.MapFrom(s => s.Id))
                    .ForMember(dest => dest.CanCreateFeedback, src => src.MapFrom(s => s.Feedback == null))
                    .ForMember(dest => dest.CanEditFeedback, src => src.MapFrom(s => s.Feedback != null));

                config.CreateMap<FeedbackInputModel, Feedback>()
                    .ForMember(dest => dest.Content, src => src.MapFrom(s => s.Content))
                    //.ForMember(dest => dest.RepairTask.Id, src => src.MapFrom(s => s.RepairTaskId))
                    .ForMember(dest => dest.UserId, src => src.MapFrom(s => s.UserId));
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
                    name: "technicianDemotePropmote",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{technicianName}"
                    );

                routes.MapRoute(
                    name: "customerDetails",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{customerName}"
                    );

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
