using MediatR;
using StudentManagementSystem.Application.Commands.Courses;
using StudentManagementSystem.Application.Common.Interfaces;

namespace StudentManagementSystem.Application.Handlers.Courses;

public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, bool>
{
    private readonly ICourseRepository _courseRepository;

    public DeleteCourseCommandHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.Id);
        if (course == null)
        {
            throw new InvalidOperationException("Course not found.");
        }

        // Check if there are enrolled students
        var enrolledCount = await _courseRepository.GetEnrolledStudentsCountAsync(request.Id);
        if (enrolledCount > 0)  
        {
            throw new InvalidOperationException("Cannot delete course with enrolled students. Please remove all students first.");
        }

        await _courseRepository.DeleteAsync(course);
        return true;
    }
}