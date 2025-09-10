using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Domain.Events.Course;

public class CourseStartedEvent : BaseDomainEvent
{
    public Course Course { get; }
    public int TeacherId { get; }
    public string TeacherName { get; }

    public CourseStartedEvent(Course course, int teacherId, string teacherName)
    {
        Course = course;
        TeacherId = teacherId;
        TeacherName = teacherName;
    }
}
