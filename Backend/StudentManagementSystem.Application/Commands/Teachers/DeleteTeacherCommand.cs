using StudentManagementSystem.Application.Common.Interfaces;

namespace StudentManagementSystem.Application.Commands.Teachers;

public record DeleteTeacherCommand : ICommand
{
    public int Id { get; init; }
}

public class DeleteTeacherCommandHandler : ICommandHandler<DeleteTeacherCommand>
{
    private readonly ITeacherRepository _teacherRepository;

    public DeleteTeacherCommandHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository.GetByIdAsync(request.Id, cancellationToken);
        if (teacher == null)
            throw new ArgumentException("Teacher not found", nameof(request.Id));

        await _teacherRepository.DeleteAsync(teacher, cancellationToken);
    }
}
