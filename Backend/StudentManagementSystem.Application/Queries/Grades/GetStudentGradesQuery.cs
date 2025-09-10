using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Queries.Grades;

public record GetStudentGradesQuery : IQuery<IEnumerable<Grade>>
{
    public int StudentId { get; init; }
}

public class GetStudentGradesQueryHandler : IQueryHandler<GetStudentGradesQuery, IEnumerable<Grade>>
{
    private readonly IGradeRepository _gradeRepository;

    public GetStudentGradesQueryHandler(IGradeRepository gradeRepository)
    {
        _gradeRepository = gradeRepository;
    }

    public async Task<IEnumerable<Grade>> Handle(GetStudentGradesQuery request, CancellationToken cancellationToken)
    {
        return await _gradeRepository.GetByStudentIdAsync(request.StudentId, cancellationToken);
    }
}
