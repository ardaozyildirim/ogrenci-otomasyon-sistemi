using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Domain.Events.Student;

public class StudentEnrolledInCourseEvent : BaseDomainEvent
{
    public int StudentId { get; }
    public int CourseId { get; }
    public string StudentName { get; }
    public string CourseName { get; }

    public StudentEnrolledInCourseEvent(int studentId, int courseId, string studentName, string courseName)
    {
        StudentId = studentId;
        CourseId = courseId;
        StudentName = studentName;
        CourseName = courseName;
    }
}
