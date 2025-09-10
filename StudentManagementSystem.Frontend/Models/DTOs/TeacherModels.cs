namespace StudentManagementSystem.Frontend.Models.DTOs;

public class TeacherDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public UserDto User { get; set; } = null!;
}

public class CreateTeacherRequest
{
    public int UserId { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
}

public class UpdateTeacherRequest
{
    public int Id { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
