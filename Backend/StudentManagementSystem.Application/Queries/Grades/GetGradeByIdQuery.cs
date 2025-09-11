using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Grades;

public class GetGradeByIdQuery : IRequest<GradeDto?>
{
    public int Id { get; }

    public GetGradeByIdQuery(int id)
    {
        Id = id;
    }
}