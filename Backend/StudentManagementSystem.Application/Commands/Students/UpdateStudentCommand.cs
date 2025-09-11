using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Commands.Students;

public record UpdateStudentCommand : ICommand
{
    public int Id { get; init; }
    public string? Department { get; init; }
    public int? Grade { get; init; }
    public string? ClassName { get; init; }
}

public class UpdateStudentCommandHandler : ICommandHandler<UpdateStudentCommand>
{
    private readonly IStudentRepository _studentRepository;

    public UpdateStudentCommandHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.Id, cancellationToken);
        if (student == null)
            throw new ArgumentException($"Student with ID {request.Id} not found", nameof(request.Id));

        student.UpdateStudentInfo(
            request.Department,
            request.Grade,
            request.ClassName);

        await _studentRepository.UpdateAsync(student, cancellationToken);
    }
}
