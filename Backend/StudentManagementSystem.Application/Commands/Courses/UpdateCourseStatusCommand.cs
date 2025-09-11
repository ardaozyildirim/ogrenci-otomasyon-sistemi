using MediatR;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Commands.Courses;

public class UpdateCourseStatusCommand : IRequest<CourseDto>
{
    public int Id { get; set; }
    public CourseStatus Status { get; set; }

    public UpdateCourseStatusCommand(int id, CourseStatus status)
    {
        Id = id;
        Status = status;
    }
}