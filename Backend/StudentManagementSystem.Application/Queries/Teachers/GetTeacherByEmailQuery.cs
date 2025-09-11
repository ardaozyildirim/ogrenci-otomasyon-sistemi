using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Teachers;

public class GetTeacherByEmailQuery : IRequest<TeacherDto?>
{
    public string Email { get; set; }

    public GetTeacherByEmailQuery(string email)
    {
        Email = email;
    }
}