using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.Interfaces;
using StudentManagementSystem.Infrastructure.Data;
using StudentManagementSystem.Infrastructure.Repositories;
using StudentManagementSystem.Infrastructure.Services;
using StudentManagementSystem.Infrastructure.UnitOfWork;

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

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // Services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPasswordHashService, PasswordHashService>();

        return services;
    }
}
