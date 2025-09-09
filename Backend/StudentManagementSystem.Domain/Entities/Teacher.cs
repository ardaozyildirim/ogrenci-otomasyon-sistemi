namespace StudentManagementSystem.Domain.Entities;

public class Teacher : BaseEntity
{
    public int UserId { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? Specialization { get; set; }
    public DateTime? HireDate { get; set; }
    
    public User User { get; set; } = null!;
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
