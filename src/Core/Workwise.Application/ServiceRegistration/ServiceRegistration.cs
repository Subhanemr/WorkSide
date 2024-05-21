﻿using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Workwise.Application.Dtos;

namespace Workwise.Application.ServiceRegistration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining(typeof(JobCreateDto)));
            return services;
        }
    }
}
