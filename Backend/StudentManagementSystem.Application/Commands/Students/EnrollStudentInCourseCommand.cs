using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Commands.Students;

public record EnrollStudentInCourseCommand : ICommand
{
    public int StudentId { get; init; }
    public int CourseId { get; init; }
}

public class EnrollStudentInCourseCommandHandler : ICommandHandler<EnrollStudentInCourseCommand>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;

    public EnrollStudentInCourseCommandHandler(IStudentRepository studentRepository, ICourseRepository courseRepository)
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public async Task Handle(EnrollStudentInCourseCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
            throw new ArgumentException("Student not found", nameof(request.StudentId));

        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course == null)
            throw new ArgumentException("Course not found", nameof(request.CourseId));

        student.EnrollInCourse(request.CourseId, course.Name);
        
        await _studentRepository.UpdateAsync(student, cancellationToken);
    }
}
