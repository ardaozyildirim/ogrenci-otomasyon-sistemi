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
                var adminUser = User.Create(
                    "Admin",
                    "User",
                    "admin@test.com",
                    _passwordHashService.HashPassword("Admin123"),
                    UserRole.Admin,
                    "+1234567890",
                    new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    "Admin Address");

                _context.Users.Add(adminUser);
                _logger.LogInformation("Created test admin user: admin@test.com");
            }

            // Create Teacher user if not exists
            if (existingTeacher == null)
            {
                var teacherUser = User.Create(
                    "John",
                    "Teacher",
                    "teacher@test.com",
                    _passwordHashService.HashPassword("Teacher123"),
                    UserRole.Teacher,
                    "+1234567891",
                    new DateTime(1985, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                    "Teacher Address");

                _context.Users.Add(teacherUser);
                await _context.SaveChangesAsync();

                // Create Teacher profile
                var teacher = Teacher.Create(
                    teacherUser.Id,
                    "T001",
                    "Computer Science",
                    "Software Engineering",
                    DateTime.UtcNow.AddYears(-2));

                _context.Teachers.Add(teacher);
                _logger.LogInformation("Created test teacher user: teacher@test.com");
            }

            // Create Student user if not exists
            if (existingStudent == null)
            {
                var studentUser = User.Create(
                    "Jane",
                    "Student",
                    "student@test.com",
                    _passwordHashService.HashPassword("Student123"),
                    UserRole.Student,
                    "+1234567892",
                    new DateTime(2000, 8, 20, 0, 0, 0, DateTimeKind.Utc),
                    "Student Address");

                _context.Users.Add(studentUser);
                await _context.SaveChangesAsync();

                // Create Student profile
                var student = Student.Create(
                    studentUser.Id,
                    "2024CS001",
                    "Computer Science",
                    85,
                    "CS-A");

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