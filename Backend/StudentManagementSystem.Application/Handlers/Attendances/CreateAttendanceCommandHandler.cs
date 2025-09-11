using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Commands.Attendances;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Handlers.Attendances;

public class CreateAttendanceCommandHandler : IRequestHandler<CreateAttendanceCommand, AttendanceDto>
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IMapper _mapper;

    public CreateAttendanceCommandHandler(IAttendanceRepository attendanceRepository, IMapper mapper)
    {
        _attendanceRepository = attendanceRepository;
        _mapper = mapper;
    }

    public async Task<AttendanceDto> Handle(CreateAttendanceCommand request, CancellationToken cancellationToken)
    {
        // Verify student is enrolled in the course
        var isEnrolled = await _attendanceRepository.StudentExistsInCourseAsync(request.StudentId, request.CourseId);
        if (!isEnrolled)
        {
            throw new InvalidOperationException("Student is not enrolled in this course.");
        }

        // Check if attendance already exists for this date
        var attendanceExists = await _attendanceRepository.AttendanceExistsForDateAsync(
            request.StudentId, request.CourseId, request.AttendanceDate);
        if (attendanceExists)
        {
            throw new InvalidOperationException("Attendance record already exists for this date.");
        }

        var attendance = new Attendance
        {
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            AttendanceDate = request.AttendanceDate == default ? DateTime.UtcNow : request.AttendanceDate,
            IsPresent = request.IsPresent,
            Notes = request.Notes
        };

        var createdAttendance = await _attendanceRepository.CreateAsync(attendance);
        return _mapper.Map<AttendanceDto>(createdAttendance);
    }
}