using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.DTOs;

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int TeacherId { get; set; }
    public CourseStatus Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Schedule { get; set; }
    public string? Location { get; set; }
    public TeacherDto Teacher { get; set; } = null!;
}
