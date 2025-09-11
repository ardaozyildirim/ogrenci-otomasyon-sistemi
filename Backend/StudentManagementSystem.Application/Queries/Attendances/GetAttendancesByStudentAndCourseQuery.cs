using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Attendances;

public class GetAttendancesByStudentAndCourseQuery : IRequest<IEnumerable<AttendanceDto>>
{
    public int StudentId { get; }
    public int CourseId { get; }

    public GetAttendancesByStudentAndCourseQuery(int studentId, int courseId)
    {
        StudentId = studentId;
        CourseId = courseId;
    }
}