using MediatR;

namespace StudentManagementSystem.Application.Commands.Courses;

public class EnrollStudentCommand : IRequest<bool>
{
    public int CourseId { get; set; }
    public int StudentId { get; set; }

    public EnrollStudentCommand(int courseId, int studentId)
    {
        CourseId = courseId;
        StudentId = studentId;
    }
}