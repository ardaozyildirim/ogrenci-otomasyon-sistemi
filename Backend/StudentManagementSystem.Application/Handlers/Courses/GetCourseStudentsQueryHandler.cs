using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Courses;

namespace StudentManagementSystem.Application.Handlers.Courses;

public class GetCourseStudentsQueryHandler : IRequestHandler<GetCourseStudentsQuery, IEnumerable<CourseStudentDto>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public GetCourseStudentsQueryHandler(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CourseStudentDto>> Handle(GetCourseStudentsQuery request, CancellationToken cancellationToken)
    {
        var students = await _courseRepository.GetCourseStudentsAsync(request.CourseId);
        return _mapper.Map<IEnumerable<CourseStudentDto>>(students);
    }
}