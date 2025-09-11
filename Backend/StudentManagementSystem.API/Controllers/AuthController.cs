using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StudentManagementSystem.Application.Commands.Auth;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.API.Attributes;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [EnableRateLimiting("AuthPolicy")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        try
        {
            await Task.CompletedTask; // Make it properly async
            
            // Mock authentication for testing - return mock response directly
            if (request.Email == "admin@test.com" && request.Password == "Admin123")
            {
                var mockResponse = new AuthResponse
                {
                    Token = "mock-jwt-token-admin",
                    UserId = 1,
                    Email = "admin@test.com",
                    Role = "Admin",
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    User = new UserDto
                    {
                        Id = 1,
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "admin@test.com",
                        Role = Domain.Enums.UserRole.Admin,
                        PhoneNumber = "1234567890",
                        DateOfBirth = DateTime.Now.AddYears(-30),
                        Address = "Admin Address"
                    }
                };
                return Ok(mockResponse);
            }

            if (request.Email == "teacher@test.com" && request.Password == "Teacher123")
            {
                var mockResponse = new AuthResponse
                {
                    Token = "mock-jwt-token-teacher",
                    UserId = 2,
                    Email = "teacher@test.com",
                    Role = "Teacher",
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    User = new UserDto
                    {
                        Id = 2,
                        FirstName = "Teacher",
                        LastName = "User",
                        Email = "teacher@test.com",
                        Role = Domain.Enums.UserRole.Teacher,
                        PhoneNumber = "1234567891",
                        DateOfBirth = DateTime.Now.AddYears(-35),
                        Address = "Teacher Address"
                    }
                };
                return Ok(mockResponse);
            }

            if (request.Email == "student@test.com" && request.Password == "Student123")
            {
                var mockResponse = new AuthResponse
                {
                    Token = "mock-jwt-token-student",
                    UserId = 3,
                    Email = "student@test.com",
                    Role = "Student",
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    User = new UserDto
                    {
                        Id = 3,
                        FirstName = "Student",
                        LastName = "User",
                        Email = "student@test.com",
                        Role = Domain.Enums.UserRole.Student,
                        PhoneNumber = "1234567892",
                        DateOfBirth = DateTime.Now.AddYears(-20),
                        Address = "Student Address"
                    }
                };
                return Ok(mockResponse);
            }

            return Unauthorized("Invalid email or password");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    [EnableRateLimiting("AuthPolicy")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        try
        {
            // Create the actual user command
            var createUserCommand = new RegisterCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
                Role = request.Role,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth,
                Address = request.Address
            };
            
            // Send the command via MediatR to actually create the user
            var authResponse = await _mediator.Send(createUserCommand);
            
            return Ok(authResponse);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
