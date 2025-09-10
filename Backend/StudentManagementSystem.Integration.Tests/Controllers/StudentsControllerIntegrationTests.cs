using System.Net;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;
using StudentManagementSystem.Infrastructure.Data;
using StudentManagementSystem.Integration.Tests.Common;

namespace StudentManagementSystem.Integration.Tests.Controllers;

public class StudentsControllerIntegrationTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly IntegrationTestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public StudentsControllerIntegrationTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAllStudents_ShouldReturnOkWithStudents()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await _client.GetAsync("/api/v1/students");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetStudentById_WithValidId_ShouldReturnStudent()
    {
        // Arrange
        var student = await SeedStudentAsync();

        // Act
        var response = await _client.GetAsync($"/api/v1/students/{student.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetStudentById_WithInvalidId_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/students/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateStudent_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var createStudentRequest = new
        {
            UserId = 1,
            StudentNumber = "2024CS999",
            Department = "Computer Science",
            Grade = 10,
            ClassName = "A"
        };

        var json = JsonConvert.SerializeObject(createStudentRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/students", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task CreateStudent_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var createStudentRequest = new
        {
            UserId = 0, // Invalid
            StudentNumber = "", // Invalid
            Department = "Computer Science"
        };

        var json = JsonConvert.SerializeObject(createStudentRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/students", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateStudent_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var student = await SeedStudentAsync();
        var updateStudentRequest = new
        {
            UserId = student.UserId,
            StudentNumber = student.StudentNumber,
            Department = "Updated Department",
            Grade = 11,
            ClassName = "B"
        };

        var json = JsonConvert.SerializeObject(updateStudentRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/v1/students/{student.Id}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteStudent_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var student = await SeedStudentAsync();

        // Act
        var response = await _client.DeleteAsync($"/api/v1/students/{student.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteStudent_WithInvalidId_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/v1/students/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task SeedTestDataAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!context.Users.Any())
        {
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
        }
    }

    private async Task<Student> SeedStudentAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User
        {
            FirstName = "Test",
            LastName = "Student",
            Email = "teststudent@example.com",
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

        context.Students.Add(student);
        await context.SaveChangesAsync();

        return student;
    }
}
