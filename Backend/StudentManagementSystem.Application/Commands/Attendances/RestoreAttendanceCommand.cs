using MediatR;

namespace StudentManagementSystem.Application.Commands.Attendances;

public class RestoreAttendanceCommand : IRequest<Unit>
{
    public int Id { get; }

    public RestoreAttendanceCommand(int id)
    {
        Id = id;
    }
}