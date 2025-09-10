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
            throw new ArgumentException("Course not found", nameof(request.Id));

        if (!string.IsNullOrWhiteSpace(request.Name))
            course.Name = request.Name;
        
        if (!string.IsNullOrWhiteSpace(request.Description))
            course.Description = request.Description;
        
        if (!string.IsNullOrWhiteSpace(request.Schedule))
            course.Schedule = request.Schedule;
        
        if (!string.IsNullOrWhiteSpace(request.Location))
            course.Location = request.Location;

        await _courseRepository.UpdateAsync(course, cancellationToken);
    }
}
