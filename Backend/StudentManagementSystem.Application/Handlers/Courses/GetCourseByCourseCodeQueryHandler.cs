using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Courses;

namespace StudentManagementSystem.Application.Handlers.Courses;

public class GetCourseByCourseCodeQueryHandler : IRequestHandler<GetCourseByCourseCodeQuery, CourseDto?>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public GetCourseByCourseCodeQueryHandler(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<CourseDto?> Handle(GetCourseByCourseCodeQuery request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByCourseCodeAsync(request.CourseCode);
        if (course == null) return null;

        var courseDto = _mapper.Map<CourseDto>(course);
        courseDto.EnrolledStudentsCount = await _courseRepository.GetEnrolledStudentsCountAsync(course.Id);
        
        return courseDto;
    }
}