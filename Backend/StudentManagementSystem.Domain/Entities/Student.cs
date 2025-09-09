namespace StudentManagementSystem.Domain.Entities;

public class Student : BaseEntity
{
    public int UserId { get; set; }
    public string StudentNumber { get; set; } = string.Empty;
    public string? Department { get; set; }
    public int? Grade { get; set; }
    public string? ClassName { get; set; }
    
    public User User { get; set; } = null!;
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
