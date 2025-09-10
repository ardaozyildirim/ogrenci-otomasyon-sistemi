using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Queries.Students;

public record GetAllStudentsQuery : IQuery<IEnumerable<Student>>
{
    public int? PageNumber { get; init; }
    public int? PageSize { get; init; }
    public string? Department { get; init; }
    public int? Grade { get; init; }
}

public class GetAllStudentsQueryHandler : IQueryHandler<GetAllStudentsQuery, IEnumerable<Student>>
{
    private readonly IStudentRepository _studentRepository;

    public GetAllStudentsQueryHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<IEnumerable<Student>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        return await _studentRepository.GetAllAsync(request.PageNumber, request.PageSize, request.Department, request.Grade, cancellationToken);
    }
}
