namespace StudentManagementSystem.Frontend.Models.DTOs;

public class GradeDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public int Score { get; set; }
    public string GradeLetter { get; set; } = string.Empty;
    public string Comments { get; set; } = string.Empty;
    public DateTime AssignedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public StudentDto Student { get; set; } = null!;
    public CourseDto Course { get; set; } = null!;
}

public class AssignGradeRequest
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public int Score { get; set; }
    public string Comments { get; set; } = string.Empty;
}

public class UpdateGradeRequest
{
    public int Id { get; set; }
    public int Score { get; set; }
    public string Comments { get; set; } = string.Empty;
}
