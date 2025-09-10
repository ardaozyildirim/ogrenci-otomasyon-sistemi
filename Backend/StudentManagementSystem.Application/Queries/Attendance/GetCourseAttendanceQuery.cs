using MediatR;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Queries.Attendance;

public class GetCourseAttendanceQuery : IRequest<IEnumerable<StudentManagementSystem.Domain.Entities.Attendance>>
{
    public int CourseId { get; set; }
    public DateTime? Date { get; set; }
}
