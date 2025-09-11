using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Attendances;

public class GetAttendanceSummaryQuery : IRequest<IEnumerable<AttendanceSummaryDto>>
{
    public int? StudentId { get; }
    public int? CourseId { get; }

    public GetAttendanceSummaryQuery(int? studentId = null, int? courseId = null)
    {
        StudentId = studentId;
        CourseId = courseId;
    }
}