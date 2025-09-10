using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Queries.Students;

public record GetStudentsByCourseQuery : IQuery<IEnumerable<Student>>
{
    public int CourseId { get; init; }
}

public class GetStudentsByCourseQueryHandler : IQueryHandler<GetStudentsByCourseQuery, IEnumerable<Student>>
{
    private readonly IStudentRepository _studentRepository;

    public GetStudentsByCourseQueryHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<IEnumerable<Student>> Handle(GetStudentsByCourseQuery request, CancellationToken cancellationToken)
    {
        return await _studentRepository.GetByCourseIdAsync(request.CourseId, cancellationToken);
    }
}
