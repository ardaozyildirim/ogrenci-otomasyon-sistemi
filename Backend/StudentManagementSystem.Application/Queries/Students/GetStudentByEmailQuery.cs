using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Students;

public class GetStudentByEmailQuery : IRequest<StudentDto?>
{
    public string Email { get; set; }

    public GetStudentByEmailQuery(string email)
    {
        Email = email;
    }
}