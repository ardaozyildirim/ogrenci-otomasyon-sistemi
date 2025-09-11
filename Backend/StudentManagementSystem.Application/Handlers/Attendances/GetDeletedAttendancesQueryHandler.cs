using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Attendances;

namespace StudentManagementSystem.Application.Handlers.Attendances;

public class GetDeletedAttendancesQueryHandler : IRequestHandler<GetDeletedAttendancesQuery, IEnumerable<AttendanceDto>>
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IMapper _mapper;

    public GetDeletedAttendancesQueryHandler(IAttendanceRepository attendanceRepository, IMapper mapper)
    {
        _attendanceRepository = attendanceRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AttendanceDto>> Handle(GetDeletedAttendancesQuery request, CancellationToken cancellationToken)
    {
        var deletedAttendances = await _attendanceRepository.GetDeletedAsync();
        return _mapper.Map<IEnumerable<AttendanceDto>>(deletedAttendances);
    }
}