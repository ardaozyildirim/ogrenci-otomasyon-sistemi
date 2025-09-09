using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Domain.Entities;

public class Course : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int TeacherId { get; set; }
    public CourseStatus Status { get; set; } = CourseStatus.NotStarted;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Schedule { get; set; }
    public string? Location { get; set; }
    
    public Teacher Teacher { get; set; } = null!;
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
