using StudentManagementSystem.Application.Common.Interfaces;

namespace StudentManagementSystem.Application.Commands.Courses;

public record UpdateCourseCommand : ICommand
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Schedule { get; init; }
    public string? Location { get; init; }
}

public class UpdateCourseCommandHandler : ICommandHandler<UpdateCourseCommand>
{
    private readonly ICourseRepository _courseRepository;

    public UpdateCourseCommandHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (course == null)
            throw new ArgumentException($"Course with ID {request.Id} not found", nameof(request.Id));

        course.UpdateCourseDetails(
            request.Name,
            request.Description,
            request.Schedule,
            request.Location);

        await _courseRepository.UpdateAsync(course, cancellationToken);
    }
}
