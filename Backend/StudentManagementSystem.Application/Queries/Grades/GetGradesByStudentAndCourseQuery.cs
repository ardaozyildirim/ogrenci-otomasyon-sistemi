using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Grades;

public class GetGradesByStudentAndCourseQuery : IRequest<IEnumerable<GradeDto>>
{
    public int StudentId { get; }
    public int CourseId { get; }

    public GetGradesByStudentAndCourseQuery(int studentId, int courseId)
    {
        StudentId = studentId;
        CourseId = courseId;
    }
}