using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Queries.Students;

public record GetStudentByIdQuery : IQuery<Student?>
{
    public int Id { get; init; }
}

public class GetStudentByIdQueryHandler : IQueryHandler<GetStudentByIdQuery, Student?>
{
    private readonly IStudentRepository _studentRepository;

    public GetStudentByIdQueryHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Student?> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _studentRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
