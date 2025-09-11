using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Attendances;

public class GetAttendancesByStudentQuery : IRequest<IEnumerable<StudentAttendanceDto>>
{
    public int StudentId { get; }

    public GetAttendancesByStudentQuery(int studentId)
    {
        StudentId = studentId;
    }
}