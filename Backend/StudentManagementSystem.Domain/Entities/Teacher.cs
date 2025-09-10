using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Entities;

public class Teacher : BaseEntity
{
    public int UserId { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? Specialization { get; set; }
    public DateTime? HireDate { get; set; }
    
    public User User { get; set; } = null!;
    public ICollection<Course> Courses { get; set; } = new List<Course>();

    public static Teacher Create(int userId, string employeeNumber, string? department = null, string? specialization = null, DateTime? hireDate = null)
    {
        return new Teacher
        {
            UserId = userId,
            EmployeeNumber = employeeNumber,
            Department = department,
            Specialization = specialization,
            HireDate = hireDate ?? DateTime.UtcNow
        };
    }

    public void AddCourse(Course course)
    {
        if (course.TeacherId != Id)
            throw new InvalidOperationException("Course must belong to this teacher");

        Courses.Add(course);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveCourse(int courseId)
    {
        var course = Courses.FirstOrDefault(c => c.Id == courseId);
        if (course != null)
        {
            Courses.Remove(course);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public IEnumerable<Course> GetActiveCourses()
    {
        return Courses.Where(c => c.Status == Enums.CourseStatus.InProgress);
    }

    public IEnumerable<Course> GetCompletedCourses()
    {
        return Courses.Where(c => c.Status == Enums.CourseStatus.Completed);
    }
}
