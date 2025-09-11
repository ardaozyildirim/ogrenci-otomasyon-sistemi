using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.Interfaces;
using StudentManagementSystem.Infrastructure.Data;
using StudentManagementSystem.Infrastructure.Repositories;
using StudentManagementSystem.Infrastructure.Services;
using StudentManagementSystem.Infrastructure.UnitOfWork;
using MediatR;
using StudentManagementSystem.Application.Mappings;
using FluentValidation;
using System.Reflection;

namespace StudentManagementSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IGradeRepository, GradeRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

               // Services
               services.AddScoped<IEmailService, EmailService>();
               services.AddScoped<IPasswordHashService, PasswordHashService>();
               services.AddScoped<IJwtTokenService, JwtTokenService>();
               services.AddScoped<ISecurityService, SecurityService>();

               // MediatR
               services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(StudentManagementSystem.Application.AssemblyReference).Assembly));

               // AutoMapper
               services.AddAutoMapper(cfg => cfg.AddMaps(typeof(StudentManagementSystem.Application.AssemblyReference).Assembly));

               // FluentValidation
               services.AddValidatorsFromAssembly(typeof(StudentManagementSystem.Application.AssemblyReference).Assembly);

               return services;
    }
}
