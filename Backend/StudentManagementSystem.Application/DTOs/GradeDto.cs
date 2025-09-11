namespace StudentManagementSystem.Application.DTOs;

public class GradeDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public string? LetterGrade { get; set; }
    public string? Comments { get; set; }
    public DateTime GradeDate { get; set; }
    public string GradeType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateGradeDto
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public decimal Score { get; set; }
    public string? LetterGrade { get; set; }
    public string? Comments { get; set; }
    public DateTime GradeDate { get; set; }
    public string GradeType { get; set; } = "Final";
}

public class UpdateGradeDto
{
    public int Id { get; set; }
    public decimal Score { get; set; }
    public string? LetterGrade { get; set; }
    public string? Comments { get; set; }
    public DateTime GradeDate { get; set; }
    public string GradeType { get; set; } = string.Empty;
}

public class StudentGradeDto
{
    public int Id { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public string? LetterGrade { get; set; }
    public string? Comments { get; set; }
    public DateTime GradeDate { get; set; }
    public string GradeType { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
}