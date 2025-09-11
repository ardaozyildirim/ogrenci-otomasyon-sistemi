using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Grades;

public class GetAllGradesQuery : IRequest<IEnumerable<GradeDto>>
{
}