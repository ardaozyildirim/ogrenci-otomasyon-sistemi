using MediatR;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Queries.Attendance;

public class GetStudentAttendanceQuery : IRequest<IEnumerable<StudentManagementSystem.Domain.Entities.Attendance>>
{
    public int StudentId { get; set; }
    public int? CourseId { get; set; }
}
