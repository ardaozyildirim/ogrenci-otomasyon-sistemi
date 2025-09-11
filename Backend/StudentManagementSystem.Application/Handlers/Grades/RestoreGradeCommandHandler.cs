using MediatR;
using StudentManagementSystem.Application.Commands.Grades;
using StudentManagementSystem.Application.Common.Interfaces;

namespace StudentManagementSystem.Application.Handlers.Grades;

public class RestoreGradeCommandHandler : IRequestHandler<RestoreGradeCommand, Unit>
{
    private readonly IGradeRepository _gradeRepository;

    public RestoreGradeCommandHandler(IGradeRepository gradeRepository)
    {
        _gradeRepository = gradeRepository;
    }

    public async Task<Unit> Handle(RestoreGradeCommand request, CancellationToken cancellationToken)
    {
        var grade = await _gradeRepository.GetByIdIncludingDeletedAsync(request.Id);
        if (grade == null || !grade.IsDeleted)
        {
            throw new InvalidOperationException("Grade not found or not deleted.");
        }

        await _gradeRepository.RestoreAsync(request.Id);
        return Unit.Value;
    }
}