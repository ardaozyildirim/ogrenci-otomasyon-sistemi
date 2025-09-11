using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Attendances;

public class GetAttendanceByIdQuery : IRequest<AttendanceDto?>
{
    public int Id { get; }

    public GetAttendanceByIdQuery(int id)
    {
        Id = id;
    }
}