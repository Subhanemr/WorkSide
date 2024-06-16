using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Workwise.Application.Abstractions.Repositories;
using Workwise.Application.Abstractions.Services;
using Workwise.Domain.Entities;
using Workwise.Infrastructure.Implementations;
using Workwise.Persistance.DAL;
using Workwise.Persistance.Implementations.Repositories;
using Workwise.Persistance.Implementations.Services;

namespace Workwise.Persistance.ServiceRegistration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddIdentity(services);
            AddUtilities(services);
            AddServices(services);
            AddRepositories(services);
            AddInterceptors(services);

            return services;
        }

        private static void AddIdentity(IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;

                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Default")));
        }

        private static void AddUtilities(IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenHandler, TokenHandler>();
        }

        private static void AddInterceptors(IServiceCollection services)
        {
            services.AddScoped<AppDbContextInitializer>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IPortfolioService, PortfolioService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ISettingsService, SettingsService>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ISettingsRepository, SettingsRepository>();
        }
    }
}
