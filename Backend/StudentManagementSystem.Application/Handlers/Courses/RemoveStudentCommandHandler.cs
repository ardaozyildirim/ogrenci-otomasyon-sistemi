using MediatR;
using StudentManagementSystem.Application.Commands.Courses;
using StudentManagementSystem.Application.Common.Interfaces;

namespace StudentManagementSystem.Application.Handlers.Courses;

public class RemoveStudentCommandHandler : IRequestHandler<RemoveStudentCommand, bool>
{
    private readonly ICourseRepository _courseRepository;

    public RemoveStudentCommandHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<bool> Handle(RemoveStudentCommand request, CancellationToken cancellationToken)
    {
        // Check if course exists
        var course = await _courseRepository.GetByIdAsync(request.CourseId);
        if (course == null)
        {
            throw new InvalidOperationException("Course not found.");
        }

        await _courseRepository.RemoveStudentAsync(request.CourseId, request.StudentId);
        return true;
    }
}