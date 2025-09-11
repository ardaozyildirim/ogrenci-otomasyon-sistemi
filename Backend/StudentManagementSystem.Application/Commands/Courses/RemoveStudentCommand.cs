using MediatR;

namespace StudentManagementSystem.Application.Commands.Courses;

public class RemoveStudentCommand : IRequest<bool>
{
    public int CourseId { get; set; }
    public int StudentId { get; set; }

    public RemoveStudentCommand(int courseId, int studentId)
    {
        CourseId = courseId;
        StudentId = studentId;
    }
}