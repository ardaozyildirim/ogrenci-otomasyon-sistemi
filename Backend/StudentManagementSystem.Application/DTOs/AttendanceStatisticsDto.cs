namespace StudentManagementSystem.Application.DTOs;

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
