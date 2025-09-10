using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Commands.Grades;

public record AssignGradeCommand : ICommand<int>
{
    public int StudentId { get; init; }
    public int CourseId { get; init; }
    public decimal Score { get; init; }
    public string? GradeType { get; init; }
    public string? Comment { get; init; }
}

public class AssignGradeCommandHandler : ICommandHandler<AssignGradeCommand, int>
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;

    public AssignGradeCommandHandler(
        IGradeRepository gradeRepository,
        IStudentRepository studentRepository,
        ICourseRepository courseRepository)
    {
        _gradeRepository = gradeRepository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<int> Handle(AssignGradeCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student == null)
            throw new ArgumentException("Student not found", nameof(request.StudentId));

        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course == null)
            throw new ArgumentException("Course not found", nameof(request.CourseId));

        var grade = Grade.Create(request.StudentId, request.CourseId, request.Score, request.GradeType, request.Comment);
        
        var createdGrade = await _gradeRepository.AddAsync(grade, cancellationToken);
        
        return createdGrade.Id;
    }
}
