using StudentManagementSystem.Domain.Events.Student;
using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Entities;

public class Student : BaseEntity
{
    public int UserId { get; set; }
    public string StudentNumber { get; set; } = string.Empty;
    public string? Department { get; set; }
    public int? Grade { get; set; }
    public string? ClassName { get; set; }
    
    public User User { get; set; } = null!;
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public static Student Create(int userId, string studentNumber, string? department = null, int? grade = null, string? className = null)
    {
        var student = new Student
        {
            UserId = userId,
            StudentNumber = studentNumber,
            Department = department,
            Grade = grade,
            ClassName = className
        };

        student.AddDomainEvent(new StudentCreatedEvent(student, studentNumber, ""));
        
        return student;
    }

    public void EnrollInCourse(int courseId, string courseName)
    {
        var enrollment = new StudentCourse
        {
            StudentId = Id,
            CourseId = courseId,
            EnrollmentDate = DateTime.UtcNow,
            IsActive = true
        };

        StudentCourses.Add(enrollment);
        AddDomainEvent(new StudentEnrolledInCourseEvent(Id, courseId, User?.FullName ?? "Unknown", courseName));
    }

    public void UnenrollFromCourse(int courseId)
    {
        var enrollment = StudentCourses.FirstOrDefault(sc => sc.CourseId == courseId && sc.IsActive);
        if (enrollment != null)
        {
            enrollment.IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public decimal CalculateGPA()
    {
        if (!Grades.Any())
            return 0;

        var totalCredits = StudentCourses.Where(sc => sc.IsActive).Sum(sc => sc.Course.Credits);
        var totalPoints = Grades.Sum(g => GetGradePoints(g.Score) * g.Course.Credits);
        
        return totalCredits > 0 ? totalPoints / totalCredits : 0;
    }

    private static decimal GetGradePoints(decimal score)
    {
        return score switch
        {
            >= 90 => 4.0m,
            >= 80 => 3.0m,
            >= 70 => 2.0m,
            >= 60 => 1.0m,
            _ => 0.0m
        };
    }
}
