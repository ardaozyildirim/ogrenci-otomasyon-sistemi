using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Queries.Courses;

public record GetAllCoursesQuery : IQuery<IEnumerable<Course>>
{
    public int? PageNumber { get; init; }
    public int? PageSize { get; init; }
    public int? TeacherId { get; init; }
    public string? Department { get; init; }
}

public class GetAllCoursesQueryHandler : IQueryHandler<GetAllCoursesQuery, IEnumerable<Course>>
{
    private readonly ICourseRepository _courseRepository;

    public GetAllCoursesQueryHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<Course>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        return await _courseRepository.GetAllAsync(request.PageNumber, request.PageSize, request.TeacherId, request.Department, cancellationToken);
    }
}
