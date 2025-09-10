using MediatR;

namespace StudentManagementSystem.Application.Commands.Attendance;

public class RecordAttendanceCommand : IRequest<int>
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
}
