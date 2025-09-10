using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Infrastructure.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StudentManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IPasswordHashService _passwordHashService;

    public AuthController(IConfiguration configuration, IPasswordHashService passwordHashService)
    {
        _configuration = configuration;
        _passwordHashService = passwordHashService;
    }

    [HttpPost("login")]
    public Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        // Mock authentication - in real app, validate against database
        if (request.Email == "admin@example.com" && request.Password == "admin123")
        {
            var token = GenerateJwtToken("admin@example.com", "Admin");
            return Task.FromResult<ActionResult<AuthResponse>>(Ok(new AuthResponse
            {
                Token = token,
                User = new UserDto
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@example.com",
                    Role = Domain.Enums.UserRole.Admin
                }
            }));
        }

        if (request.Email == "teacher@example.com" && request.Password == "teacher123")
        {
            var token = GenerateJwtToken("teacher@example.com", "Teacher");
            return Task.FromResult<ActionResult<AuthResponse>>(Ok(new AuthResponse
            {
                Token = token,
                User = new UserDto
                {
                    Id = 2,
                    FirstName = "Teacher",
                    LastName = "User",
                    Email = "teacher@example.com",
                    Role = Domain.Enums.UserRole.Teacher
                }
            }));
        }

        if (request.Email == "student@example.com" && request.Password == "student123")
        {
            var token = GenerateJwtToken("student@example.com", "Student");
            return Task.FromResult<ActionResult<AuthResponse>>(Ok(new AuthResponse
            {
                Token = token,
                User = new UserDto
                {
                    Id = 3,
                    FirstName = "Student",
                    LastName = "User",
                    Email = "student@example.com",
                    Role = Domain.Enums.UserRole.Student
                }
            }));
        }

        return Task.FromResult<ActionResult<AuthResponse>>(Unauthorized("Invalid credentials"));
    }

    private string GenerateJwtToken(string email, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
