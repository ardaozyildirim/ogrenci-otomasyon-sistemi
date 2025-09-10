using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Attendance;

public class GetAttendanceStatisticsQuery : IRequest<AttendanceStatisticsDto>
{
    public int StudentId { get; set; }
    public int? CourseId { get; set; }
}
