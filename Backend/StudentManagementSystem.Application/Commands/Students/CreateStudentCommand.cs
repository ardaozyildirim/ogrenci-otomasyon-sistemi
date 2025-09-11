using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Commands.Students;

public class CreateStudentCommand : IRequest<StudentDto>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public DateTime EnrollmentDate { get; set; }
}