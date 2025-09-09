namespace StudentManagementSystem.Application.DTOs;

public class TeacherDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? Specialization { get; set; }
    public DateTime? HireDate { get; set; }
    public UserDto User { get; set; } = null!;
}
