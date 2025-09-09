namespace StudentManagementSystem.Application.DTOs;

public class StudentDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string StudentNumber { get; set; } = string.Empty;
    public string? Department { get; set; }
    public int? Grade { get; set; }
    public string? ClassName { get; set; }
    public UserDto User { get; set; } = null!;
}
