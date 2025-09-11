using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Commands.Students;

public record CreateStudentCommand : ICommand<int>
{
    public int UserId { get; init; }
    public string StudentNumber { get; init; } = string.Empty;
    public string? Department { get; init; }
    public int? Grade { get; init; }
    public string? ClassName { get; init; }
}

public class CreateStudentCommandHandler : ICommandHandler<CreateStudentCommand, int>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IUserRepository _userRepository;

    public CreateStudentCommandHandler(IStudentRepository studentRepository, IUserRepository userRepository)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<int> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        await ValidateUserExists(request.UserId, cancellationToken);
        
        var student = CreateStudentFromCommand(request);
        var savedStudent = await _studentRepository.AddAsync(student, cancellationToken);
        
        return savedStudent.Id;
    }

    private async Task ValidateUserExists(int userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
            throw new ArgumentException($"User with ID {userId} not found", nameof(userId));
    }

    private static Student CreateStudentFromCommand(CreateStudentCommand command)
    {
        return Student.Create(
            command.UserId, 
            command.StudentNumber, 
            command.Department, 
            command.Grade, 
            command.ClassName);
    }
}
