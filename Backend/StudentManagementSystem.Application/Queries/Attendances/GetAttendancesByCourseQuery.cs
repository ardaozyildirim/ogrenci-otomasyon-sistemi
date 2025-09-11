using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Attendances;

public class GetAttendancesByCourseQuery : IRequest<IEnumerable<AttendanceDto>>
{
    public int CourseId { get; }

    public GetAttendancesByCourseQuery(int courseId)
    {
        CourseId = courseId;
    }
}