using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Domain.Events.Grade;

public class GradeAssignedEvent : BaseDomainEvent
{
    public Entities.Grade Grade { get; }
    public string StudentName { get; }
    public string CourseName { get; }
    public decimal Score { get; }

    public GradeAssignedEvent(Entities.Grade grade, string studentName, string courseName)
    {
        Grade = grade;
        StudentName = studentName;
        CourseName = courseName;
        Score = grade.Score;
    }
}
