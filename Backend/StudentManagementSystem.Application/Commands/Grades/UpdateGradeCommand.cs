using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Commands.Grades;

public class UpdateGradeCommand : IRequest<GradeDto>
{
    public int Id { get; set; }
    public decimal Score { get; set; }
    public string? LetterGrade { get; set; }
    public string? Comments { get; set; }
    public DateTime GradeDate { get; set; }
    public string GradeType { get; set; } = string.Empty;
}