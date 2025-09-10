using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Domain.Events.Student;

public class StudentCreatedEvent : BaseDomainEvent
{
    public Entities.Student Student { get; }
    public string StudentNumber { get; }
    public string Email { get; }

    public StudentCreatedEvent(Entities.Student student, string studentNumber, string email)
    {
        Student = student;
        StudentNumber = studentNumber;
        Email = email;
    }
}
