using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Commands.Attendances;

public class UpdateAttendanceCommand : IRequest<AttendanceDto>
{
    public int Id { get; set; }
    public DateTime AttendanceDate { get; set; }
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
}