using MediatR;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Commands.Auth;

public class RegisterCommand : IRequest<AuthResponse>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Student;
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
}
