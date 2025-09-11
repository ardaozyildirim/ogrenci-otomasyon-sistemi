using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Grades;

namespace StudentManagementSystem.Application.Handlers.Grades;

public class GetGradeByIdQueryHandler : IRequestHandler<GetGradeByIdQuery, GradeDto?>
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IMapper _mapper;

    public GetGradeByIdQueryHandler(IGradeRepository gradeRepository, IMapper mapper)
    {
        _gradeRepository = gradeRepository;
        _mapper = mapper;
    }

    public async Task<GradeDto?> Handle(GetGradeByIdQuery request, CancellationToken cancellationToken)
    {
        var grade = await _gradeRepository.GetByIdAsync(request.Id);
        return grade != null ? _mapper.Map<GradeDto>(grade) : null;
    }
}