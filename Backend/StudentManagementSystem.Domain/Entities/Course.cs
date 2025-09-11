using StudentManagementSystem.Domain.Common;

namespace StudentManagementSystem.Domain.Entities;

public class Course : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int TeacherId { get; set; }
    public int Capacity { get; set; } = 30;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Teacher Teacher { get; set; } = null!;
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
}