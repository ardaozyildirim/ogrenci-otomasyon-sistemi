using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Courses;

public class GetCoursesByTeacherQuery : IRequest<IEnumerable<CourseDto>>
{
    public int TeacherId { get; set; }

    public GetCoursesByTeacherQuery(int teacherId)
    {
        TeacherId = teacherId;
    }
}