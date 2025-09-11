using StudentManagementSystem.Domain.Common;

namespace StudentManagementSystem.Domain.Entities;

public class StudentCourse : BaseEntity
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
}