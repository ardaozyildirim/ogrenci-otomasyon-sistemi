using StudentManagementSystem.Application.Common.Interfaces;

namespace StudentManagementSystem.Application.Commands.Students;

public record DeleteStudentCommand : ICommand
{
    public int Id { get; init; }
}

public class DeleteStudentCommandHandler : ICommandHandler<DeleteStudentCommand>
{
    private readonly IStudentRepository _studentRepository;

    public DeleteStudentCommandHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.Id, cancellationToken);
        if (student == null)
            throw new ArgumentException("Student not found", nameof(request.Id));

        await _studentRepository.DeleteAsync(student, cancellationToken);
    }
}
