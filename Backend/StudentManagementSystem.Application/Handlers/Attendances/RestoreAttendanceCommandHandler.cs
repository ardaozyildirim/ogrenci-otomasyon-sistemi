using MediatR;
using StudentManagementSystem.Application.Commands.Attendances;
using StudentManagementSystem.Application.Common.Interfaces;

namespace StudentManagementSystem.Application.Handlers.Attendances;

public class RestoreAttendanceCommandHandler : IRequestHandler<RestoreAttendanceCommand, Unit>
{
    private readonly IAttendanceRepository _attendanceRepository;

    public RestoreAttendanceCommandHandler(IAttendanceRepository attendanceRepository)
    {
        _attendanceRepository = attendanceRepository;
    }

    public async Task<Unit> Handle(RestoreAttendanceCommand request, CancellationToken cancellationToken)
    {
        var attendance = await _attendanceRepository.GetByIdIncludingDeletedAsync(request.Id);
        if (attendance == null || !attendance.IsDeleted)
        {
            throw new InvalidOperationException("Attendance record not found or not deleted.");
        }

        await _attendanceRepository.RestoreAsync(request.Id);
        return Unit.Value;
    }
}