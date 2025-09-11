using MediatR;

namespace StudentManagementSystem.Application.Commands.Teachers;

public class DeleteTeacherCommand : IRequest<bool>
{
    public int Id { get; set; }

    public DeleteTeacherCommand(int id)
    {
        Id = id;
    }
}