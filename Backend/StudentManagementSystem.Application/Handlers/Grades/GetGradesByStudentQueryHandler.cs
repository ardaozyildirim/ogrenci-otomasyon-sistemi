using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Grades;

namespace StudentManagementSystem.Application.Handlers.Grades;

public class GetGradesByStudentQueryHandler : IRequestHandler<GetGradesByStudentQuery, IEnumerable<StudentGradeDto>>
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IMapper _mapper;

    public GetGradesByStudentQueryHandler(IGradeRepository gradeRepository, IMapper mapper)
    {
        _gradeRepository = gradeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StudentGradeDto>> Handle(GetGradesByStudentQuery request, CancellationToken cancellationToken)
    {
        var grades = await _gradeRepository.GetByStudentIdAsync(request.StudentId);
        return _mapper.Map<IEnumerable<StudentGradeDto>>(grades);
    }
}