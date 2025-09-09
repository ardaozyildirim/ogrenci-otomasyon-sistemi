namespace StudentManagementSystem.Domain.Entities;

public class Attendance : BaseEntity
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
    
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
}
