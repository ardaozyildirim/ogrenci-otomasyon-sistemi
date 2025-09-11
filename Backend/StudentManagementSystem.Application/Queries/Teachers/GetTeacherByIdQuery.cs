using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Teachers;

public class GetTeacherByIdQuery : IRequest<TeacherDto?>
{
    public int Id { get; set; }

    public GetTeacherByIdQuery(int id)
    {
        Id = id;
    }
}