using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Grades;

public class GetDeletedGradesQuery : IRequest<IEnumerable<GradeDto>>
{
}