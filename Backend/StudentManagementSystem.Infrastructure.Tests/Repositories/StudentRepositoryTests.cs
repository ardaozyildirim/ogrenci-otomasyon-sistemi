using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;
using StudentManagementSystem.Infrastructure.Data;
using StudentManagementSystem.Infrastructure.Repositories;

namespace StudentManagementSystem.Infrastructure.Tests.Repositories;

public class StudentRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly StudentRepository _repository;

    public StudentRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new StudentRepository(_context);
    }

    [Fact]
    public async Task GetByStudentNumberAsync_ExistingStudent_ShouldReturnStudent()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Role = UserRole.Student
        };

        var student = new Student
        {
            Id = 1,
            UserId = 1,
            StudentNumber = "2024CS001",
            Department = "Computer Science",
            User = user
        };

        _context.Users.Add(user);
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByStudentNumberAsync("2024CS001");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("2024CS001", result.StudentNumber);
        Assert.Equal("Computer Science", result.Department);
        Assert.NotNull(result.User);
    }

    [Fact]
    public async Task GetByStudentNumberAsync_NonExistingStudent_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByStudentNumberAsync("2024CS999");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUserIdAsync_ExistingStudent_ShouldReturnStudent()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Role = UserRole.Student
        };

        var student = new Student
        {
            Id = 1,
            UserId = 1,
            StudentNumber = "2024CS001",
            Department = "Computer Science",
            User = user
        };

        _context.Users.Add(user);
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByUserIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.UserId);
        Assert.Equal("2024CS001", result.StudentNumber);
    }

    [Fact]
    public async Task GetByDepartmentAsync_ExistingStudents_ShouldReturnStudents()
    {
        // Arrange
        var user1 = new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Role = UserRole.Student };
        var user2 = new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Role = UserRole.Student };

        var student1 = new Student { Id = 1, UserId = 1, StudentNumber = "2024CS001", Department = "Computer Science", User = user1 };
        var student2 = new Student { Id = 2, UserId = 2, StudentNumber = "2024CS002", Department = "Computer Science", User = user2 };

        _context.Users.AddRange(user1, user2);
        _context.Students.AddRange(student1, student2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByDepartmentAsync("Computer Science");

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, s => Assert.Equal("Computer Science", s.Department));
    }

    [Fact]
    public async Task StudentNumberExistsAsync_ExistingStudent_ShouldReturnTrue()
    {
        // Arrange
        var user = new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Role = UserRole.Student };
        var student = new Student { Id = 1, UserId = 1, StudentNumber = "2024CS001", Department = "Computer Science", User = user };

        _context.Users.Add(user);
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.StudentNumberExistsAsync("2024CS001");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task StudentNumberExistsAsync_NonExistingStudent_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.StudentNumberExistsAsync("2024CS999");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetAllAsync_WithPagination_ShouldReturnPagedResults()
    {
        // Arrange
        var users = new List<User>();
        var students = new List<Student>();

        for (int i = 1; i <= 5; i++)
        {
            var user = new User { Id = i, FirstName = $"Student{i}", LastName = "Doe", Email = $"student{i}@example.com", Role = UserRole.Student };
            var student = new Student { Id = i, UserId = i, StudentNumber = $"2024CS{i:D3}", Department = "Computer Science", User = user };
            users.Add(user);
            students.Add(student);
        }

        _context.Users.AddRange(users);
        _context.Students.AddRange(students);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync(pageNumber: 1, pageSize: 3);

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_WithDepartmentFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var user1 = new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Role = UserRole.Student };
        var user2 = new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Role = UserRole.Student };

        var student1 = new Student { Id = 1, UserId = 1, StudentNumber = "2024CS001", Department = "Computer Science", User = user1 };
        var student2 = new Student { Id = 2, UserId = 2, StudentNumber = "2024MATH001", Department = "Mathematics", User = user2 };

        _context.Users.AddRange(user1, user2);
        _context.Students.AddRange(student1, student2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync(department: "Computer Science");

        // Assert
        Assert.Single(result);
        Assert.Equal("Computer Science", result.First().Department);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
