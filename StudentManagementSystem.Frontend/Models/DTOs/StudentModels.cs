namespace StudentManagementSystem.Frontend.Models.DTOs;

public class StudentDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string StudentNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public int Grade { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public UserDto User { get; set; } = null!;
}

public class CreateStudentRequest
{
    public int UserId { get; set; }
    public string StudentNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public int Grade { get; set; }
}

public class UpdateStudentRequest
{
    public int Id { get; set; }
    public string StudentNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public int Grade { get; set; }
    public bool IsActive { get; set; }
}
