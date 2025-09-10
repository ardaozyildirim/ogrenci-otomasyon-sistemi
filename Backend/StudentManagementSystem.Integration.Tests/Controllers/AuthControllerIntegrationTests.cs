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

public class AuthControllerIntegrationTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly IntegrationTestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AuthControllerIntegrationTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var user = await SeedUserAsync("test@example.com", "password123");
        var loginRequest = new
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var json = JsonConvert.SerializeObject(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/auth/login", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();
        
        var authResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
        authResponse.Should().NotBeNull();
        ((string)authResponse!.token).Should().NotBeNullOrEmpty();
        ((int)authResponse.userId).Should().Be(user.Id);
        ((string)authResponse.email).Should().Be("test@example.com");
    }

    [Fact]
    public async Task Login_WithInvalidEmail_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginRequest = new
        {
            Email = "nonexistent@example.com",
            Password = "password123"
        };

        var json = JsonConvert.SerializeObject(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/auth/login", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ShouldReturnUnauthorized()
    {
        // Arrange
        await SeedUserAsync("test@example.com", "password123");
        var loginRequest = new
        {
            Email = "test@example.com",
            Password = "wrongpassword"
        };

        var json = JsonConvert.SerializeObject(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/auth/login", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithInvalidRequest_ShouldReturnBadRequest()
    {
        // Arrange
        var loginRequest = new
        {
            Email = "", // Invalid
            Password = "" // Invalid
        };

        var json = JsonConvert.SerializeObject(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/auth/login", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var registerRequest = new
        {
            FirstName = "New",
            LastName = "User",
            Email = "newuser@example.com",
            Password = "password123",
            Role = "Student",
            PhoneNumber = "1234567890",
            DateOfBirth = DateTime.Now.AddYears(-20),
            Address = "Test Address"
        };

        var json = JsonConvert.SerializeObject(registerRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/auth/register", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Register_WithExistingEmail_ShouldReturnBadRequest()
    {
        // Arrange
        await SeedUserAsync("existing@example.com", "password123");
        var registerRequest = new
        {
            FirstName = "Another",
            LastName = "User",
            Email = "existing@example.com", // Already exists
            Password = "password123",
            Role = "Student"
        };

        var json = JsonConvert.SerializeObject(registerRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/auth/register", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var registerRequest = new
        {
            FirstName = "", // Invalid
            LastName = "", // Invalid
            Email = "invalid-email", // Invalid
            Password = "123", // Too short
            Role = "InvalidRole" // Invalid
        };

        var json = JsonConvert.SerializeObject(registerRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/auth/register", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private async Task<User> SeedUserAsync(string email, string password)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = email,
            Role = UserRole.Student,
            PasswordHash = password // In real scenario, this would be hashed
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }
}
