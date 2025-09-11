using StudentManagementSystem.Domain.Enums;
using StudentManagementSystem.Domain.Events.Course;
using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Entities;

public class Course : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int Credits { get; private set; }
    public int TeacherId { get; private set; }
    public CourseStatus Status { get; private set; } = CourseStatus.NotStarted;
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Schedule { get; private set; }
    public string? Location { get; private set; }
    
    public Teacher Teacher { get; set; } = null!;
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public static Course Create(string name, string code, int credits, int teacherId, 
        string? description = null, string? schedule = null, string? location = null)
    {
        ValidateCourseParams(name, code, credits);

        return new Course
        {
            Name = name.Trim(),
            Code = code.ToUpperInvariant().Trim(),
            Credits = credits,
            TeacherId = teacherId,
            Description = description?.Trim(),
            Schedule = schedule?.Trim(),
            Location = location?.Trim(),
            Status = CourseStatus.NotStarted
        };
    }

    private static void ValidateCourseParams(string name, string code, int credits)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Course name is required", nameof(name));
            
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Course code is required", nameof(code));
            
        if (credits <= 0)
            throw new ArgumentException("Credits must be greater than 0", nameof(credits));
    }

    public void StartCourse()
    {
        EnsureCourseCanBeStarted();

        Status = CourseStatus.InProgress;
        StartDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        RaiseCourseStartedEvent();
    }

    private void EnsureCourseCanBeStarted()
    {
        if (Status != CourseStatus.NotStarted)
            throw new InvalidOperationException("Course can only be started if it hasn't been started yet");
    }

    private void RaiseCourseStartedEvent()
    {
        var teacherName = Teacher?.User?.FullName ?? "Unknown Teacher";
        AddDomainEvent(new CourseStartedEvent(this, TeacherId, teacherName));
    }

    public void CompleteCourse()
    {
        EnsureCourseCanBeCompleted();

        Status = CourseStatus.Completed;
        EndDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    private void EnsureCourseCanBeCompleted()
    {
        if (Status != CourseStatus.InProgress)
            throw new InvalidOperationException("Course can only be completed if it's currently in progress");
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
        EnsureStudentCanEnroll();
        
        var existingEnrollment = FindStudentEnrollment(studentId);
        if (existingEnrollment != null)
        {
            ReactivateExistingEnrollment(existingEnrollment);
        }
        else
        {
            CreateNewEnrollment(studentId);
        }

        UpdatedAt = DateTime.UtcNow;
    }

    private void EnsureStudentCanEnroll()
    {
        if (Status != CourseStatus.InProgress)
            throw new InvalidOperationException("Students can only enroll in courses that are currently in progress");
    }

    private StudentCourse? FindStudentEnrollment(int studentId)
    {
        return StudentCourses.FirstOrDefault(sc => sc.StudentId == studentId);
    }

    private void ReactivateExistingEnrollment(StudentCourse enrollment)
    {
        if (enrollment.IsActive)
            throw new InvalidOperationException("Student is already enrolled in this course");
        
        enrollment.IsActive = true;
        enrollment.EnrollmentDate = DateTime.UtcNow;
    }

    private void CreateNewEnrollment(int studentId)
    {
        StudentCourses.Add(new StudentCourse
        {
            StudentId = studentId,
            CourseId = Id,
            EnrollmentDate = DateTime.UtcNow,
            IsActive = true
        });
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

    public void UpdateCourseDetails(string? name = null, string? description = null, 
        string? schedule = null, string? location = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name.Trim();
            
        Description = description?.Trim();
        Schedule = schedule?.Trim();
        Location = location?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
}
