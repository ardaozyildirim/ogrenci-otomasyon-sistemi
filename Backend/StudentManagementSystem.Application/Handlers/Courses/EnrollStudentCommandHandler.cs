using MediatR;
using StudentManagementSystem.Application.Commands.Courses;
using StudentManagementSystem.Application.Common.Interfaces;

namespace StudentManagementSystem.Application.Handlers.Courses;

public class EnrollStudentCommandHandler : IRequestHandler<EnrollStudentCommand, bool>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IStudentRepository _studentRepository;

    public EnrollStudentCommandHandler(ICourseRepository courseRepository, IStudentRepository studentRepository)
    {
        _courseRepository = courseRepository;
        _studentRepository = studentRepository;
    }

    public async Task<bool> Handle(EnrollStudentCommand request, CancellationToken cancellationToken)
    {
        // Check if course exists
        var course = await _courseRepository.GetByIdAsync(request.CourseId);
        if (course == null)
        {
            throw new InvalidOperationException("Course not found.");
        }

        // Check if student exists
        var student = await _studentRepository.GetByIdAsync(request.StudentId);
        if (student == null)
        {
            throw new InvalidOperationException("Student not found.");
        }

        await _courseRepository.EnrollStudentAsync(request.CourseId, request.StudentId);
        return true;
    }
}