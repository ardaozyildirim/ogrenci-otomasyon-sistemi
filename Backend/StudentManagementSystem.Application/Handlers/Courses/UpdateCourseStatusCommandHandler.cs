using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Commands.Courses;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Handlers.Courses;

public class UpdateCourseStatusCommandHandler : IRequestHandler<UpdateCourseStatusCommand, CourseDto>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public UpdateCourseStatusCommandHandler(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<CourseDto> Handle(UpdateCourseStatusCommand request, CancellationToken cancellationToken)
    {
        var existingCourse = await _courseRepository.GetByIdAsync(request.Id);
        if (existingCourse == null)
        {
            throw new InvalidOperationException("Course not found.");
        }

        existingCourse.Status = request.Status;
        existingCourse.UpdatedAt = DateTime.UtcNow;

        var updatedCourse = await _courseRepository.UpdateAsync(existingCourse);
        var courseDto = _mapper.Map<CourseDto>(updatedCourse);
        courseDto.EnrolledStudentsCount = await _courseRepository.GetEnrolledStudentsCountAsync(updatedCourse.Id);
        
        return courseDto;
    }
}