using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Teachers;

public class GetAllTeachersQuery : IRequest<IEnumerable<TeacherDto>>
{
}