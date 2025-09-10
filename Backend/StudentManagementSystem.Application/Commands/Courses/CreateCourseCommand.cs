using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Commands.Courses;

public record CreateCourseCommand : ICommand<int>
{
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public int Credits { get; init; }
    public int TeacherId { get; init; }
    public string? Description { get; init; }
    public string? Schedule { get; init; }
    public string? Location { get; init; }
}

public class CreateCourseCommandHandler : ICommandHandler<CreateCourseCommand, int>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ITeacherRepository _teacherRepository;

    public CreateCourseCommandHandler(ICourseRepository courseRepository, ITeacherRepository teacherRepository)
    {
        _courseRepository = courseRepository;
        _teacherRepository = teacherRepository;
    }

    public async Task<int> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository.GetByIdAsync(request.TeacherId, cancellationToken);
        if (teacher == null)
            throw new ArgumentException("Teacher not found", nameof(request.TeacherId));

        var course = Course.Create(request.Name, request.Code, request.Credits, request.TeacherId, request.Description, request.Schedule, request.Location);
        
        var createdCourse = await _courseRepository.AddAsync(course, cancellationToken);
        
        return createdCourse.Id;
    }
}
