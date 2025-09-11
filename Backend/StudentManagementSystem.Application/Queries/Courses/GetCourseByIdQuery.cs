using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Courses;

public class GetCourseByIdQuery : IRequest<CourseDto?>
{
    public int Id { get; set; }

    public GetCourseByIdQuery(int id)
    {
        Id = id;
    }
}