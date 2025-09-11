using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Students;

public class GetAllStudentsQuery : IRequest<IEnumerable<StudentDto>>
{
}