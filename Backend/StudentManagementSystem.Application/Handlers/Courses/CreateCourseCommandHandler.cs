using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Commands.Courses;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Handlers.Courses;

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CourseDto>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;

    public CreateCourseCommandHandler(ICourseRepository courseRepository, ITeacherRepository teacherRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _teacherRepository = teacherRepository;
        _mapper = mapper;
    }

    public async Task<CourseDto> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        // Check if course code already exists
        if (await _courseRepository.CourseCodeExistsAsync(request.CourseCode))
        {
            throw new InvalidOperationException("A course with this course code already exists.");
        }

        // Check if teacher exists
        var teacher = await _teacherRepository.GetByIdAsync(request.TeacherId);
        if (teacher == null)
        {
            throw new InvalidOperationException("Teacher not found.");
        }

        var course = new Course
        {
            Name = request.Name,
            CourseCode = request.CourseCode,
            Description = request.Description,
            Credits = request.Credits,
            TeacherId = request.TeacherId,
            Capacity = request.Capacity,
            Status = CourseStatus.NotStarted,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var createdCourse = await _courseRepository.AddAsync(course);
        var courseDto = _mapper.Map<CourseDto>(createdCourse);
        courseDto.EnrolledStudentsCount = 0;
        
        return courseDto;
    }
}