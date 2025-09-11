using StudentManagementSystem.Domain.Events.Student;
using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Entities;

public class Student : BaseEntity
{
    public int UserId { get; private set; }
    public string StudentNumber { get; private set; } = string.Empty;
    public string? Department { get; private set; }
    public int? Grade { get; private set; }
    public string? ClassName { get; private set; }
    
    public User User { get; set; } = null!;
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public static Student Create(int userId, string studentNumber, string? department = null, 
        int? grade = null, string? className = null)
    {
        ValidateStudentCreationParams(userId, studentNumber);

        var student = new Student
        {
            UserId = userId,
            StudentNumber = studentNumber.Trim(),
            Department = department?.Trim(),
            Grade = grade,
            ClassName = className?.Trim()
        };

        student.RaiseStudentCreatedEvent(studentNumber);
        return student;
    }

    private static void ValidateStudentCreationParams(int userId, string studentNumber)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be a positive number", nameof(userId));
            
        if (string.IsNullOrWhiteSpace(studentNumber))
            throw new ArgumentException("Student number is required", nameof(studentNumber));
    }

    private void RaiseStudentCreatedEvent(string studentNumber)
    {
        AddDomainEvent(new StudentCreatedEvent(this, studentNumber, ""));
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
        if (!HasAnyGrades())
            return 0;

        var activeEnrollments = GetActiveEnrollments();
        var totalCredits = activeEnrollments.Sum(sc => sc.Course.Credits);
        var totalPoints = CalculateTotalGradePoints();
        
        return totalCredits > 0 ? totalPoints / totalCredits : 0;
    }

    private bool HasAnyGrades() => Grades.Any();
    
    private IEnumerable<StudentCourse> GetActiveEnrollments()
    {
        return StudentCourses.Where(sc => sc.IsActive);
    }

    private decimal CalculateTotalGradePoints()
    {
        return Grades.Sum(g => ConvertScoreToGradePoints(g.Score) * g.Course.Credits);
    }

    private static decimal ConvertScoreToGradePoints(decimal score)
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

    public void UpdateStudentInfo(string? department = null, int? grade = null, string? className = null)
    {
        Department = department?.Trim();
        Grade = grade;
        ClassName = className?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
}
