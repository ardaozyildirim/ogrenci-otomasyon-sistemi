using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Commands.Grades;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Handlers.Grades;

public class CreateGradeCommandHandler : IRequestHandler<CreateGradeCommand, GradeDto>
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IMapper _mapper;

    public CreateGradeCommandHandler(IGradeRepository gradeRepository, IMapper mapper)
    {
        _gradeRepository = gradeRepository;
        _mapper = mapper;
    }

    public async Task<GradeDto> Handle(CreateGradeCommand request, CancellationToken cancellationToken)
    {
        // Verify student is enrolled in the course
        var isEnrolled = await _gradeRepository.StudentExistsInCourseAsync(request.StudentId, request.CourseId);
        if (!isEnrolled)
        {
            throw new InvalidOperationException("Student is not enrolled in this course.");
        }

        var grade = new Grade
        {
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            Score = request.Score,
            LetterGrade = request.LetterGrade,
            Comments = request.Comments,
            GradeDate = request.GradeDate == default ? DateTime.UtcNow : request.GradeDate,
            GradeType = request.GradeType
        };

        var createdGrade = await _gradeRepository.CreateAsync(grade);
        return _mapper.Map<GradeDto>(createdGrade);
    }
}