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
        _studentRepository = studentRepository;
        _userRepository = userRepository;
    }

    public async Task<int> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            throw new ArgumentException("User not found", nameof(request.UserId));

        var student = Student.Create(request.UserId, request.StudentNumber, request.Department, request.Grade, request.ClassName);
        
        var createdStudent = await _studentRepository.AddAsync(student, cancellationToken);
        
        return createdStudent.Id;
    }
}
