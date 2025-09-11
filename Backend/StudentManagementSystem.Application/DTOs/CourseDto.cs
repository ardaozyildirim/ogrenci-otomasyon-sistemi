using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.DTOs;

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public CourseStatus Status { get; set; }
    public bool IsActive { get; set; }
    public int EnrolledStudentsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateCourseDto
{
    public string Name { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int TeacherId { get; set; }
    public int Capacity { get; set; } = 30;
}

public class UpdateCourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int TeacherId { get; set; }
    public int Capacity { get; set; }
    public CourseStatus Status { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateCourseStatusDto
{
    public int Id { get; set; }
    public CourseStatus Status { get; set; }
}

public class EnrollStudentDto
{
    public int CourseId { get; set; }
    public int StudentId { get; set; }
}

public class CourseStudentDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
}