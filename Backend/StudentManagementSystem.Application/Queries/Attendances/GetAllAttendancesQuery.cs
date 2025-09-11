using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Queries.Attendances;

public class GetAllAttendancesQuery : IRequest<IEnumerable<AttendanceDto>>
{
}