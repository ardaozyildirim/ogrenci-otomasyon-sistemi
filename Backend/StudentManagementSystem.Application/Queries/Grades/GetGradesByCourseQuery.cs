using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Grades;

public class GetGradesByCourseQuery : IRequest<IEnumerable<GradeDto>>
{
    public int CourseId { get; }

    public GetGradesByCourseQuery(int courseId)
    {
        CourseId = courseId;
    }
}