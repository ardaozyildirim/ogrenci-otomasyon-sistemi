using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.API.Services;

public class DatabaseSeederService
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHashService _passwordHashService;
    private readonly ILogger<DatabaseSeederService> _logger;

    public DatabaseSeederService(
        IApplicationDbContext context,
        IPasswordHashService passwordHashService,
        ILogger<DatabaseSeederService> logger)
    {
        _context = context;
        _passwordHashService = passwordHashService;
        _logger = logger;
    }

    public async Task SeedTestUsersAsync()
    {
        try
        {
            // Check if test users already exist
            var existingAdmin = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == "admin@test.com");
            var existingTeacher = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == "teacher@test.com");
            var existingStudent = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == "student@test.com");

            // Create Admin user if not exists
            if (existingAdmin == null)
            {
                var adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@test.com",
                    PasswordHash = _passwordHashService.HashPassword("Admin123"),
                    Role = UserRole.Admin,
                    PhoneNumber = "+1234567890",
                    DateOfBirth = new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Address = "Admin Address",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(adminUser);
                _logger.LogInformation("Created test admin user: admin@test.com");
            }

            // Create Teacher user if not exists
            if (existingTeacher == null)
            {
                var teacherUser = new User
                {
                    FirstName = "John",
                    LastName = "Teacher",
                    Email = "teacher@test.com",
                    PasswordHash = _passwordHashService.HashPassword("Teacher123"),
                    Role = UserRole.Teacher,
                    PhoneNumber = "+1234567891",
                    DateOfBirth = new DateTime(1985, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                    Address = "Teacher Address",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(teacherUser);
                await _context.SaveChangesAsync();

                // Create Teacher profile
                var teacher = new Teacher
                {
                    UserId = teacherUser.Id,
                    EmployeeNumber = "T001",
                    Department = "Computer Science",
                    Specialization = "Software Engineering",
                    HireDate = DateTime.UtcNow.AddYears(-2),
                    CreatedAt = DateTime.UtcNow
                };

                _context.Teachers.Add(teacher);
                _logger.LogInformation("Created test teacher user: teacher@test.com");
            }

            // Create Student user if not exists
            if (existingStudent == null)
            {
                var studentUser = new User
                {
                    FirstName = "Jane",
                    LastName = "Student",
                    Email = "student@test.com",
                    PasswordHash = _passwordHashService.HashPassword("Student123"),
                    Role = UserRole.Student,
                    PhoneNumber = "+1234567892",
                    DateOfBirth = new DateTime(2000, 8, 20, 0, 0, 0, DateTimeKind.Utc),
                    Address = "Student Address",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(studentUser);
                await _context.SaveChangesAsync();

                // Create Student profile
                var student = new Student
                {
                    UserId = studentUser.Id,
                    StudentNumber = "2024CS001",
                    Department = "Computer Science",
                    Grade = 85,
                    ClassName = "CS-A",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Students.Add(student);
                _logger.LogInformation("Created test student user: student@test.com");
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Test users seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while seeding test users");
            throw;
        }
    }
}