using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Queries.Courses;

public record GetCourseByIdQuery : IQuery<Course?>
{
    public int Id { get; init; }
}

public class GetCourseByIdQueryHandler : IQueryHandler<GetCourseByIdQuery, Course?>
{
    private readonly ICourseRepository _courseRepository;

    public GetCourseByIdQueryHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<Course?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        return await _courseRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
