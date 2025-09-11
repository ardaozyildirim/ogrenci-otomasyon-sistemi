using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Courses;

public class GetCourseStudentsQuery : IRequest<IEnumerable<CourseStudentDto>>
{
    public int CourseId { get; set; }

    public GetCourseStudentsQuery(int courseId)
    {
        CourseId = courseId;
    }
}