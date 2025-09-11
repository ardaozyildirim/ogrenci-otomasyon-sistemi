using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Grades;

namespace StudentManagementSystem.Application.Handlers.Grades;

public class GetGradesByStudentAndCourseQueryHandler : IRequestHandler<GetGradesByStudentAndCourseQuery, IEnumerable<GradeDto>>
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IMapper _mapper;

    public GetGradesByStudentAndCourseQueryHandler(IGradeRepository gradeRepository, IMapper mapper)
    {
        _gradeRepository = gradeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GradeDto>> Handle(GetGradesByStudentAndCourseQuery request, CancellationToken cancellationToken)
    {
        var grades = await _gradeRepository.GetByStudentAndCourseAsync(request.StudentId, request.CourseId);
        return _mapper.Map<IEnumerable<GradeDto>>(grades);
    }
}