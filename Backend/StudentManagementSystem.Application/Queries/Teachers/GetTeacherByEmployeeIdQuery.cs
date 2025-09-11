using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Teachers;

public class GetTeacherByEmployeeIdQuery : IRequest<TeacherDto?>
{
    public string EmployeeId { get; set; }

    public GetTeacherByEmployeeIdQuery(string employeeId)
    {
        EmployeeId = employeeId;
    }
}