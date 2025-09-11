using StudentManagementSystem.Domain.Common;

namespace StudentManagementSystem.Domain.Entities;

public class Grade : BaseEntity
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public decimal Score { get; set; }
    public string? LetterGrade { get; set; }
    public string? Comments { get; set; }
    public DateTime GradeDate { get; set; }
    public string GradeType { get; set; } = "Final"; // Midterm, Final, Assignment, etc.

    // Navigation properties
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
}