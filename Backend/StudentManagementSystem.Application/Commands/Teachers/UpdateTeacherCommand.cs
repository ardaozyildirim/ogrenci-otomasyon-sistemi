using StudentManagementSystem.Application.Common.Interfaces;

namespace StudentManagementSystem.Application.Commands.Teachers;

public record UpdateTeacherCommand : ICommand
{
    public int Id { get; init; }
    public string? Department { get; init; }
    public string? Specialization { get; init; }
}

public class UpdateTeacherCommandHandler : ICommandHandler<UpdateTeacherCommand>
{
    private readonly ITeacherRepository _teacherRepository;

    public UpdateTeacherCommandHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository.GetByIdAsync(request.Id, cancellationToken);
        if (teacher == null)
            throw new ArgumentException("Teacher not found", nameof(request.Id));

        if (!string.IsNullOrWhiteSpace(request.Department))
            teacher.Department = request.Department;
        
        if (!string.IsNullOrWhiteSpace(request.Specialization))
            teacher.Specialization = request.Specialization;

        await _teacherRepository.UpdateAsync(teacher, cancellationToken);
    }
}
