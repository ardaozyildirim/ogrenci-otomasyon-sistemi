using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Application.Queries.Courses;

namespace StudentManagementSystem.Application.Handlers.Courses;

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, IEnumerable<CourseDto>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public GetAllCoursesQueryHandler(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CourseDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.GetAllAsync();
        var courseDtos = _mapper.Map<IEnumerable<CourseDto>>(courses);

        // Add enrolled students count for each course
        foreach (var courseDto in courseDtos)
        {
            courseDto.EnrolledStudentsCount = await _courseRepository.GetEnrolledStudentsCountAsync(courseDto.Id);
        }

        return courseDtos;
    }
}