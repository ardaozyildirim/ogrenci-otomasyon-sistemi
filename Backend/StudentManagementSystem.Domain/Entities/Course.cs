using StudentManagementSystem.Domain.Enums;
using StudentManagementSystem.Domain.Events.Course;
using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Entities;

public class Course : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int TeacherId { get; set; }
    public CourseStatus Status { get; set; } = CourseStatus.NotStarted;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Schedule { get; set; }
    public string? Location { get; set; }
    
    public Teacher Teacher { get; set; } = null!;
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public static Course Create(string name, string code, int credits, int teacherId, string? description = null, string? schedule = null, string? location = null)
    {
        if (credits <= 0)
            throw new ArgumentException("Credits must be greater than 0", nameof(credits));

        return new Course
        {
            Name = name,
            Code = code,
            Credits = credits,
            TeacherId = teacherId,
            Description = description,
            Schedule = schedule,
            Location = location,
            Status = CourseStatus.NotStarted
        };
    }

    public void StartCourse()
    {
        if (Status != CourseStatus.NotStarted)
            throw new InvalidOperationException("Course can only be started if it's not started yet");

        Status = CourseStatus.InProgress;
        StartDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CourseStartedEvent(this, TeacherId, Teacher?.User?.FullName ?? "Unknown Teacher"));
    }

    public void CompleteCourse()
    {
        if (Status != CourseStatus.InProgress)
            throw new InvalidOperationException("Course can only be completed if it's in progress");

        Status = CourseStatus.Completed;
        EndDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void CancelCourse()
    {
        if (Status == CourseStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed course");

        Status = CourseStatus.Cancelled;
        EndDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void EnrollStudent(int studentId)
    {
        if (Status != CourseStatus.InProgress)
            throw new InvalidOperationException("Students can only be enrolled in active courses");

        var existingEnrollment = StudentCourses.FirstOrDefault(sc => sc.StudentId == studentId);
        if (existingEnrollment != null)
        {
            if (existingEnrollment.IsActive)
                throw new InvalidOperationException("Student is already enrolled in this course");
            
            existingEnrollment.IsActive = true;
            existingEnrollment.EnrollmentDate = DateTime.UtcNow;
        }
        else
        {
            StudentCourses.Add(new StudentCourse
            {
                StudentId = studentId,
                CourseId = Id,
                EnrollmentDate = DateTime.UtcNow,
                IsActive = true
            });
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void UnenrollStudent(int studentId)
    {
        var enrollment = StudentCourses.FirstOrDefault(sc => sc.StudentId == studentId && sc.IsActive);
        if (enrollment != null)
        {
            enrollment.IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public IEnumerable<Student> GetEnrolledStudents()
    {
        return StudentCourses
            .Where(sc => sc.IsActive)
            .Select(sc => sc.Student);
    }

    public int GetEnrolledStudentCount()
    {
        return StudentCourses.Count(sc => sc.IsActive);
    }
}
