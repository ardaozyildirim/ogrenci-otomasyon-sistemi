
namespace StudentManagementSystem.Domain.Entities;

public class Attendance : BaseEntity
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
    
    // Navigation properties
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;

    public static Attendance Create(int studentId, int courseId, DateTime date, bool isPresent, string? notes = null)
    {
        return new Attendance
        {
            StudentId = studentId,
            CourseId = courseId,
            Date = date,
            IsPresent = isPresent,
            Notes = notes
        };
    }

    public void Update(bool isPresent, string? notes = null)
    {
        IsPresent = isPresent;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }
}