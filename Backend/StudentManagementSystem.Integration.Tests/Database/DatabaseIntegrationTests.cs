using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;
using StudentManagementSystem.Infrastructure.Data;
using StudentManagementSystem.Integration.Tests.Common;

namespace StudentManagementSystem.Integration.Tests.Database;

public class DatabaseIntegrationTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly IntegrationTestWebApplicationFactory _factory;

    public DatabaseIntegrationTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateUser_ShouldPersistToDatabase()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            Role = UserRole.Student,
            PasswordHash = "hashed_password",
            PhoneNumber = "1234567890",
            DateOfBirth = DateTime.Now.AddYears(-20),
            Address = "Test Address"
        };

        // Act
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Assert
        var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        savedUser.Should().NotBeNull();
        savedUser!.FirstName.Should().Be("Test");
        savedUser.LastName.Should().Be("User");
        savedUser.Email.Should().Be("test@example.com");
        savedUser.Role.Should().Be(UserRole.Student);
    }

    [Fact]
    public async Task CreateStudent_ShouldPersistToDatabase()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User
        {
            FirstName = "Student",
            LastName = "Test",
            Email = "student@example.com",
            Role = UserRole.Student,
            PasswordHash = "hashed_password"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var student = new Student
        {
            UserId = user.Id,
            StudentNumber = "2024CS001",
            Department = "Computer Science",
            Grade = 10,
            ClassName = "A"
        };

        // Act
        context.Students.Add(student);
        await context.SaveChangesAsync();

        // Assert
        var savedStudent = await context.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.StudentNumber == "2024CS001");

        savedStudent.Should().NotBeNull();
        savedStudent!.StudentNumber.Should().Be("2024CS001");
        savedStudent.Department.Should().Be("Computer Science");
        savedStudent.Grade.Should().Be(10);
        savedStudent.User.Should().NotBeNull();
        savedStudent.User.Email.Should().Be("student@example.com");
    }

    [Fact]
    public async Task CreateTeacher_ShouldPersistToDatabase()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User
        {
            FirstName = "Teacher",
            LastName = "Test",
            Email = "teacher@example.com",
            Role = UserRole.Teacher,
            PasswordHash = "hashed_password"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var teacher = new Teacher
        {
            UserId = user.Id,
            EmployeeNumber = "EMP001",
            Department = "Computer Science",
            Specialization = "Software Engineering",
            HireDate = DateTime.Now
        };

        // Act
        context.Teachers.Add(teacher);
        await context.SaveChangesAsync();

        // Assert
        var savedTeacher = await context.Teachers
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.EmployeeNumber == "EMP001");

        savedTeacher.Should().NotBeNull();
        savedTeacher!.EmployeeNumber.Should().Be("EMP001");
        savedTeacher.Department.Should().Be("Computer Science");
        savedTeacher.Specialization.Should().Be("Software Engineering");
        savedTeacher.User.Should().NotBeNull();
        savedTeacher.User.Email.Should().Be("teacher@example.com");
    }

    [Fact]
    public async Task CreateCourse_ShouldPersistToDatabase()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User
        {
            FirstName = "Teacher",
            LastName = "Test",
            Email = "teacher@example.com",
            Role = UserRole.Teacher,
            PasswordHash = "hashed_password"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var teacher = new Teacher
        {
            UserId = user.Id,
            EmployeeNumber = "EMP001",
            Department = "Computer Science",
            Specialization = "Software Engineering",
            HireDate = DateTime.Now
        };

        context.Teachers.Add(teacher);
        await context.SaveChangesAsync();

        var course = new Course
        {
            Name = "Data Structures",
            Code = "CS101",
            Credits = 3,
            TeacherId = teacher.Id,
            Description = "Introduction to data structures",
            Schedule = "Mon, Wed, Fri 10:00-11:00",
            Location = "Room 101"
        };

        // Act
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        // Assert
        var savedCourse = await context.Courses
            .Include(c => c.Teacher)
            .ThenInclude(t => t.User)
            .FirstOrDefaultAsync(c => c.Code == "CS101");

        savedCourse.Should().NotBeNull();
        savedCourse!.Name.Should().Be("Data Structures");
        savedCourse.Code.Should().Be("CS101");
        savedCourse.Credits.Should().Be(3);
        savedCourse.Teacher.Should().NotBeNull();
        savedCourse.Teacher.EmployeeNumber.Should().Be("EMP001");
    }

    [Fact]
    public async Task SoftDelete_ShouldMarkAsDeleted()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            Role = UserRole.Student,
            PasswordHash = "hashed_password"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        user.IsDeleted = true;
        await context.SaveChangesAsync();

        // Assert
        var deletedUser = await context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email == "test@example.com");

        deletedUser.Should().NotBeNull();
        deletedUser!.IsDeleted.Should().BeTrue();

        // Verify it's not returned in normal queries
        var activeUser = await context.Users
            .FirstOrDefaultAsync(u => u.Email == "test@example.com");

        activeUser.Should().BeNull();
    }

    [Fact]
    public async Task AuditFields_ShouldBeSetAutomatically()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            Role = UserRole.Student,
            PasswordHash = "hashed_password"
        };

        // Act
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Assert
        var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        savedUser.Should().NotBeNull();
        savedUser!.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        savedUser.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }
}
