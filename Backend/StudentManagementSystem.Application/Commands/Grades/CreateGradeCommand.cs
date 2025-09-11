using MediatR;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Commands.Grades;

public class CreateGradeCommand : IRequest<GradeDto>
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public decimal Score { get; set; }
    public string? LetterGrade { get; set; }
    public string? Comments { get; set; }
    public DateTime GradeDate { get; set; }
    public string GradeType { get; set; } = "Final";
}