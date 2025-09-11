using MediatR;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Commands.Courses;

public class CreateCourseCommand : IRequest<CourseDto>
{
    public string Name { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int TeacherId { get; set; }
    public int Capacity { get; set; } = 30;
}