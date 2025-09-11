using MediatR;
using StudentManagementSystem.Application.Commands.Grades;
using StudentManagementSystem.Application.Common.Interfaces;

namespace StudentManagementSystem.Application.Handlers.Grades;

public class DeleteGradeCommandHandler : IRequestHandler<DeleteGradeCommand, Unit>
{
    private readonly IGradeRepository _gradeRepository;

    public DeleteGradeCommandHandler(IGradeRepository gradeRepository)
    {
        _gradeRepository = gradeRepository;
    }

    public async Task<Unit> Handle(DeleteGradeCommand request, CancellationToken cancellationToken)
    {
        var exists = await _gradeRepository.ExistsAsync(request.Id);
        if (!exists)
        {
            throw new InvalidOperationException("Grade not found.");
        }

        await _gradeRepository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}