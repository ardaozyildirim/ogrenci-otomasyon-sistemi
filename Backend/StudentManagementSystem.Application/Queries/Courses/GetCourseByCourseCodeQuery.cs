using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Courses;

public class GetCourseByCourseCodeQuery : IRequest<CourseDto?>
{
    public string CourseCode { get; set; }

    public GetCourseByCourseCodeQuery(string courseCode)
    {
        CourseCode = courseCode;
    }
}