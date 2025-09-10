using MediatR;

namespace StudentManagementSystem.Application.Commands.Attendance;

public class UpdateAttendanceCommand : IRequest
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
}
