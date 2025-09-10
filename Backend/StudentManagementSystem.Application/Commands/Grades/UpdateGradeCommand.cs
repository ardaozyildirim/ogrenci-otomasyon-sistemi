using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Commands.Grades;

public record UpdateGradeCommand : ICommand
{
    public int Id { get; init; }
    public decimal Score { get; init; }
    public string? Comment { get; init; }
}

public class UpdateGradeCommandHandler : ICommandHandler<UpdateGradeCommand>
{
    private readonly IGradeRepository _gradeRepository;

    public UpdateGradeCommandHandler(IGradeRepository gradeRepository)
    {
        _gradeRepository = gradeRepository;
    }

    public async Task Handle(UpdateGradeCommand request, CancellationToken cancellationToken)
    {
        var grade = await _gradeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (grade == null)
            throw new ArgumentException("Grade not found", nameof(request.Id));

        grade.UpdateScore(request.Score, request.Comment);
        
        await _gradeRepository.UpdateAsync(grade, cancellationToken);
    }
}
