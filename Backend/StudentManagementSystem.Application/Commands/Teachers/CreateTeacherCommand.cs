using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Commands.Teachers;

public record CreateTeacherCommand : ICommand<int>
{
    public int UserId { get; init; }
    public string EmployeeNumber { get; init; } = string.Empty;
    public string? Department { get; init; }
    public string? Specialization { get; init; }
    public DateTime? HireDate { get; init; }
}

public class CreateTeacherCommandHandler : ICommandHandler<CreateTeacherCommand, int>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUserRepository _userRepository;

    public CreateTeacherCommandHandler(ITeacherRepository teacherRepository, IUserRepository userRepository)
    {
        _teacherRepository = teacherRepository;
        _userRepository = userRepository;
    }

    public async Task<int> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            throw new ArgumentException("User not found", nameof(request.UserId));

        var teacher = Teacher.Create(request.UserId, request.EmployeeNumber, request.Department, request.Specialization, request.HireDate);
        
        var createdTeacher = await _teacherRepository.AddAsync(teacher, cancellationToken);
        
        return createdTeacher.Id;
    }
}
