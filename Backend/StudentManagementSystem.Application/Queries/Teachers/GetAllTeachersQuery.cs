using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Queries.Teachers;

public record GetAllTeachersQuery : IQuery<IEnumerable<Teacher>>
{
    public int? PageNumber { get; init; }
    public int? PageSize { get; init; }
    public string? Department { get; init; }
}

public class GetAllTeachersQueryHandler : IQueryHandler<GetAllTeachersQuery, IEnumerable<Teacher>>
{
    private readonly ITeacherRepository _teacherRepository;

    public GetAllTeachersQueryHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<IEnumerable<Teacher>> Handle(GetAllTeachersQuery request, CancellationToken cancellationToken)
    {
        return await _teacherRepository.GetAllAsync(request.PageNumber, request.PageSize, request.Department, cancellationToken);
    }
}
