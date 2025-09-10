namespace StudentManagementSystem.Application.DTOs;

public class GradeDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public decimal Score { get; set; }
    public string? Comment { get; set; }
    public DateTime GradeDate { get; set; }
    public string? GradeType { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string LetterGrade { get; set; } = string.Empty;
    public bool IsPassingGrade { get; set; }
}
