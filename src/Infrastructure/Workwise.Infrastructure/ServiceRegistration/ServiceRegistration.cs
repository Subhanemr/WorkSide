using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using a = Workwise.Infrastructure.Implementations;
using Workwise.Application.Abstractions.Services;
using Workwise.Infrastructure.Implementations;

namespace Workwise.Infrastructure.ServiceRegistration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication();

            AddJwtBearer(services, configuration);

            services.AddScoped<ITokenHandler, a.TokenHandler>();
            services.AddScoped<ICLoudService, CloudService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }

        private static void AddJwtBearer(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["TokenOptions:Issuer"],
                    ValidAudience = configuration["TokenOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenOptions:SecurityKey"])),
                    LifetimeValidator = (_, expired, token, _) => token != null ? expired > DateTime.UtcNow : false


                };
            });
        }
    }
}
