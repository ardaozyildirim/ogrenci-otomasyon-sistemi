using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Courses;

public class GetAllCoursesQuery : IRequest<IEnumerable<CourseDto>>
{
}