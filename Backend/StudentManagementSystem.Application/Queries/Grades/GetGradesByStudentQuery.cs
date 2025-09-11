using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Grades;

public class GetGradesByStudentQuery : IRequest<IEnumerable<StudentGradeDto>>
{
    public int StudentId { get; }

    public GetGradesByStudentQuery(int studentId)
    {
        StudentId = studentId;
    }
}