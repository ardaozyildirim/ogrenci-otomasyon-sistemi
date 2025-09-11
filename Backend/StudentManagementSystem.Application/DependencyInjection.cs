using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace StudentManagementSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR registration
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // AutoMapper registration
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // FluentValidation registration
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}