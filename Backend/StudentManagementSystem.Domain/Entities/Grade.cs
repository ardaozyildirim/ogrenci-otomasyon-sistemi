namespace StudentManagementSystem.Domain.Entities;

public class Grade : BaseEntity
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public decimal Score { get; set; }
    public string? Comment { get; set; }
    public DateTime GradeDate { get; set; } = DateTime.UtcNow;
    public string? GradeType { get; set; } // Midterm, Final, Assignment, etc.
    
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
}
