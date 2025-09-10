using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Queries.Teachers;

public record GetTeacherByIdQuery : IQuery<Teacher?>
{
    public int Id { get; init; }
}

public class GetTeacherByIdQueryHandler : IQueryHandler<GetTeacherByIdQuery, Teacher?>
{
    private readonly ITeacherRepository _teacherRepository;

    public GetTeacherByIdQueryHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<Teacher?> Handle(GetTeacherByIdQuery request, CancellationToken cancellationToken)
    {
        return await _teacherRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
