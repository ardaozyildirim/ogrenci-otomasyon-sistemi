using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Students;

public class GetStudentByIdQuery : IRequest<StudentDto?>
{
    public int Id { get; set; }

    public GetStudentByIdQuery(int id)
    {
        Id = id;
    }
}