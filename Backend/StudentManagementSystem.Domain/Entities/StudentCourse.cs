namespace StudentManagementSystem.Domain.Entities;

public class StudentCourse : BaseEntity
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
}
