using AutoMapper;
using MediatR;
using StudentManagementSystem.Application.Commands.Grades;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Handlers.Grades;

public class UpdateGradeCommandHandler : IRequestHandler<UpdateGradeCommand, GradeDto>
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IMapper _mapper;

    public UpdateGradeCommandHandler(IGradeRepository gradeRepository, IMapper mapper)
    {
        _gradeRepository = gradeRepository;
        _mapper = mapper;
    }

    public async Task<GradeDto> Handle(UpdateGradeCommand request, CancellationToken cancellationToken)
    {
        var existingGrade = await _gradeRepository.GetByIdAsync(request.Id);
        if (existingGrade == null)
        {
            throw new InvalidOperationException("Grade not found.");
        }

        existingGrade.Score = request.Score;
        existingGrade.LetterGrade = request.LetterGrade;
        existingGrade.Comments = request.Comments;
        existingGrade.GradeDate = request.GradeDate;
        existingGrade.GradeType = request.GradeType;

        var updatedGrade = await _gradeRepository.UpdateAsync(existingGrade);
        return _mapper.Map<GradeDto>(updatedGrade);
    }
}