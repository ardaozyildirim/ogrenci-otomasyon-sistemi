using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Commands.Courses;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Handlers.Courses;

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, CourseDto>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;

    public UpdateCourseCommandHandler(ICourseRepository courseRepository, ITeacherRepository teacherRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _teacherRepository = teacherRepository;
        _mapper = mapper;
    }

    public async Task<CourseDto> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var existingCourse = await _courseRepository.GetByIdAsync(request.Id);
        if (existingCourse == null)
        {
            throw new InvalidOperationException("Course not found.");
        }

        // Check if course code is being changed and if the new course code already exists
        if (request.CourseCode != existingCourse.CourseCode && await _courseRepository.CourseCodeExistsAsync(request.CourseCode))
        {
            throw new InvalidOperationException("A course with this course code already exists.");
        }

        // Check if teacher exists
        var teacher = await _teacherRepository.GetByIdAsync(request.TeacherId);
        if (teacher == null)
        {
            throw new InvalidOperationException("Teacher not found.");
        }

        // Update course properties
        existingCourse.Name = request.Name;
        existingCourse.CourseCode = request.CourseCode;
        existingCourse.Description = request.Description;
        existingCourse.Credits = request.Credits;
        existingCourse.TeacherId = request.TeacherId;
        existingCourse.Capacity = request.Capacity;
        existingCourse.Status = request.Status;
        existingCourse.IsActive = request.IsActive;
        existingCourse.UpdatedAt = DateTime.UtcNow;

        var updatedCourse = await _courseRepository.UpdateAsync(existingCourse);
        var courseDto = _mapper.Map<CourseDto>(updatedCourse);
        courseDto.EnrolledStudentsCount = await _courseRepository.GetEnrolledStudentsCountAsync(updatedCourse.Id);
        
        return courseDto;
    }
}