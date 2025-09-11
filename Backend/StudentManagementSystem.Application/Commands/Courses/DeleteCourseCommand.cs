using MediatR;

namespace StudentManagementSystem.Application.Commands.Courses;

public class DeleteCourseCommand : IRequest<bool>
{
    public int Id { get; set; }

    public DeleteCourseCommand(int id)
    {
        Id = id;
    }
}