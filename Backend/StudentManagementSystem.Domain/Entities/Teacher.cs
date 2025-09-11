using StudentManagementSystem.Domain.Common;

namespace StudentManagementSystem.Domain.Entities;

public class Teacher : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Department { get; set; } = string.Empty;
    public string? Specialty { get; set; }
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<Course> Courses { get; set; } = new List<Course>();

    public string FullName => $"{FirstName} {LastName}";
}