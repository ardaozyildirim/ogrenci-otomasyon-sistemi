using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Commands.Attendance;

public class RecordAttendanceCommand : IRequest<int>
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
}

public class RecordAttendanceCommandHandler : IRequestHandler<RecordAttendanceCommand, int>
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;

    public RecordAttendanceCommandHandler(
        IAttendanceRepository attendanceRepository,
        IStudentRepository studentRepository,
        ICourseRepository courseRepository)
    {
        _attendanceRepository = attendanceRepository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<int> Handle(RecordAttendanceCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
            throw new ArgumentException("Student not found", nameof(request.StudentId));

        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course == null)
            throw new ArgumentException("Course not found", nameof(request.CourseId));

        var attendance = Domain.Entities.Attendance.Create(request.StudentId, request.CourseId, request.Date, request.IsPresent, request.Notes);
        
        var createdAttendance = await _attendanceRepository.AddAsync(attendance, cancellationToken);
        
        return createdAttendance.Id;
    }
}
