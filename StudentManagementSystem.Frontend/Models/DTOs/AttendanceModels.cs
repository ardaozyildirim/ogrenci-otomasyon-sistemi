namespace StudentManagementSystem.Frontend.Models.DTOs;

public class AttendanceDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public string? StudentName { get; set; }
    public string? CourseName { get; set; }
}

public class RecordAttendanceRequest
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
}

public class UpdateAttendanceRequest
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
}

public class AttendanceStatisticsDto
{
    public int StudentId { get; set; }
    public int? CourseId { get; set; }
    public int TotalClasses { get; set; }
    public int PresentClasses { get; set; }
    public int AbsentClasses { get; set; }
    public decimal AttendancePercentage { get; set; }
    public string? StudentName { get; set; }
    public string? CourseName { get; set; }
}
