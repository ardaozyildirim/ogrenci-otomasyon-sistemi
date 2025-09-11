using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Grades;

namespace StudentManagementSystem.Application.Handlers.Grades;

public class GetAllGradesQueryHandler : IRequestHandler<GetAllGradesQuery, IEnumerable<GradeDto>>
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IMapper _mapper;

    public GetAllGradesQueryHandler(IGradeRepository gradeRepository, IMapper mapper)
    {
        _gradeRepository = gradeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GradeDto>> Handle(GetAllGradesQuery request, CancellationToken cancellationToken)
    {
        var grades = await _gradeRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<GradeDto>>(grades);
    }
}