using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Commands.Attendances;

public class CreateAttendanceCommand : IRequest<AttendanceDto>
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
}