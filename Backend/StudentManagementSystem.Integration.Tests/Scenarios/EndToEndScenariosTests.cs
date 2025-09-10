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

namespace StudentManagementSystem.Integration.Tests.Scenarios;

public class EndToEndScenariosTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly IntegrationTestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public EndToEndScenariosTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CompleteStudentLifecycle_ShouldWorkEndToEnd()
    {
        // Step 1: Register a new student
        var registerRequest = new
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            Role = "Student",
            PhoneNumber = "1234567890",
            DateOfBirth = DateTime.Now.AddYears(-20),
            Address = "123 Main St"
        };

        var registerJson = JsonConvert.SerializeObject(registerRequest);
        var registerContent = new StringContent(registerJson, Encoding.UTF8, "application/json");

        var registerResponse = await _client.PostAsync("/api/v1/auth/register", registerContent);
        registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Step 2: Login to get JWT token
        var loginRequest = new
        {
            Email = "john.doe@example.com",
            Password = "password123"
        };

        var loginJson = JsonConvert.SerializeObject(loginRequest);
        var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

        var loginResponse = await _client.PostAsync("/api/v1/auth/login", loginContent);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginContentString = await loginResponse.Content.ReadAsStringAsync();
        var authResponse = JsonConvert.DeserializeObject<dynamic>(loginContentString);
        authResponse.Should().NotBeNull();
        var token = (string)authResponse!.token;

        // Step 3: Create student record
        var createStudentRequest = new
        {
            UserId = (int)authResponse.userId,
            StudentNumber = "2024CS001",
            Department = "Computer Science",
            Grade = 10,
            ClassName = "A"
        };

        var studentJson = JsonConvert.SerializeObject(createStudentRequest);
        var studentContent = new StringContent(studentJson, Encoding.UTF8, "application/json");
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var createStudentResponse = await _client.PostAsync("/api/v1/students", studentContent);
        createStudentResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Step 4: Get student information
        var getStudentResponse = await _client.GetAsync("/api/v1/students");
        getStudentResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var studentsContent = await getStudentResponse.Content.ReadAsStringAsync();
        studentsContent.Should().NotBeNullOrEmpty();

        // Step 5: Update student information
        var updateStudentRequest = new
        {
            UserId = (int)authResponse.userId,
            StudentNumber = "2024CS001",
            Department = "Computer Science",
            Grade = 11,
            ClassName = "B"
        };

        var updateJson = JsonConvert.SerializeObject(updateStudentRequest);
        var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");

        var updateResponse = await _client.PutAsync("/api/v1/students/1", updateContent);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Step 6: Delete student record
        var deleteResponse = await _client.DeleteAsync("/api/v1/students/1");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task TeacherCourseManagement_ShouldWorkEndToEnd()
    {
        // Step 1: Register a teacher
        var registerRequest = new
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Password = "password123",
            Role = "Teacher",
            PhoneNumber = "0987654321",
            DateOfBirth = DateTime.Now.AddYears(-30),
            Address = "456 Oak Ave"
        };

        var registerJson = JsonConvert.SerializeObject(registerRequest);
        var registerContent = new StringContent(registerJson, Encoding.UTF8, "application/json");

        var registerResponse = await _client.PostAsync("/api/v1/auth/register", registerContent);
        registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Step 2: Login to get JWT token
        var loginRequest = new
        {
            Email = "jane.smith@example.com",
            Password = "password123"
        };

        var loginJson = JsonConvert.SerializeObject(loginRequest);
        var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

        var loginResponse = await _client.PostAsync("/api/v1/auth/login", loginContent);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginContentString = await loginResponse.Content.ReadAsStringAsync();
        var authResponse = JsonConvert.DeserializeObject<dynamic>(loginContentString);
        authResponse.Should().NotBeNull();
        var token = (string)authResponse!.token;

        // Step 3: Create teacher record
        var createTeacherRequest = new
        {
            UserId = (int)authResponse.userId,
            EmployeeNumber = "EMP001",
            Department = "Computer Science",
            Specialization = "Software Engineering",
            HireDate = DateTime.Now
        };

        var teacherJson = JsonConvert.SerializeObject(createTeacherRequest);
        var teacherContent = new StringContent(teacherJson, Encoding.UTF8, "application/json");
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var createTeacherResponse = await _client.PostAsync("/api/v1/teachers", teacherContent);
        createTeacherResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Step 4: Create a course
        var createCourseRequest = new
        {
            Name = "Data Structures",
            Code = "CS101",
            Credits = 3,
            TeacherId = 1,
            Description = "Introduction to data structures",
            Schedule = "Mon, Wed, Fri 10:00-11:00",
            Location = "Room 101"
        };

        var courseJson = JsonConvert.SerializeObject(createCourseRequest);
        var courseContent = new StringContent(courseJson, Encoding.UTF8, "application/json");

        var createCourseResponse = await _client.PostAsync("/api/v1/courses", courseContent);
        createCourseResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Step 5: Get all courses
        var getCoursesResponse = await _client.GetAsync("/api/v1/courses");
        getCoursesResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var coursesContent = await getCoursesResponse.Content.ReadAsStringAsync();
        coursesContent.Should().NotBeNullOrEmpty();

        // Step 6: Update course
        var updateCourseRequest = new
        {
            Name = "Advanced Data Structures",
            Code = "CS101",
            Credits = 4,
            TeacherId = 1,
            Description = "Advanced data structures and algorithms",
            Schedule = "Mon, Wed, Fri 10:00-11:00",
            Location = "Room 102"
        };

        var updateCourseJson = JsonConvert.SerializeObject(updateCourseRequest);
        var updateCourseContent = new StringContent(updateCourseJson, Encoding.UTF8, "application/json");

        var updateCourseResponse = await _client.PutAsync("/api/v1/courses/1", updateCourseContent);
        updateCourseResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UnauthorizedAccess_ShouldBeBlocked()
    {
        // Try to access protected endpoint without token
        var response = await _client.GetAsync("/api/v1/students");
        
        // Should return unauthorized or forbidden
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task InvalidToken_ShouldBeRejected()
    {
        // Set invalid token
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "invalid_token");

        var response = await _client.GetAsync("/api/v1/students");
        
        // Should return unauthorized
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RateLimiting_ShouldWork()
    {
        // Make multiple requests quickly to test rate limiting
        var tasks = new List<Task<HttpResponseMessage>>();
        
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(_client.GetAsync("/api/v1/students"));
        }

        var responses = await Task.WhenAll(tasks);
        
        // At least some requests should be rate limited
        var rateLimitedResponses = responses.Where(r => r.StatusCode == HttpStatusCode.TooManyRequests);
        rateLimitedResponses.Should().NotBeEmpty();
    }
}
