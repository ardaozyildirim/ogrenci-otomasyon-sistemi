using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Grades;

namespace StudentManagementSystem.Application.Handlers.Grades;

public class GetDeletedGradesQueryHandler : IRequestHandler<GetDeletedGradesQuery, IEnumerable<GradeDto>>
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IMapper _mapper;

    public GetDeletedGradesQueryHandler(IGradeRepository gradeRepository, IMapper mapper)
    {
        _gradeRepository = gradeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GradeDto>> Handle(GetDeletedGradesQuery request, CancellationToken cancellationToken)
    {
        var deletedGrades = await _gradeRepository.GetDeletedAsync();
        return _mapper.Map<IEnumerable<GradeDto>>(deletedGrades);
    }
}