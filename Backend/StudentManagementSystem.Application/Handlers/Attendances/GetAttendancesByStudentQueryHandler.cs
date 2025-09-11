using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Attendances;

namespace StudentManagementSystem.Application.Handlers.Attendances;

public class GetAttendancesByStudentQueryHandler : IRequestHandler<GetAttendancesByStudentQuery, IEnumerable<StudentAttendanceDto>>
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IMapper _mapper;

    public GetAttendancesByStudentQueryHandler(IAttendanceRepository attendanceRepository, IMapper mapper)
    {
        _attendanceRepository = attendanceRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StudentAttendanceDto>> Handle(GetAttendancesByStudentQuery request, CancellationToken cancellationToken)
    {
        var attendances = await _attendanceRepository.GetByStudentIdAsync(request.StudentId);
        return _mapper.Map<IEnumerable<StudentAttendanceDto>>(attendances);
    }
}