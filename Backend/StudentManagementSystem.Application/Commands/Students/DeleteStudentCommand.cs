using MediatR;

namespace StudentManagementSystem.Application.Commands.Students;

public class DeleteStudentCommand : IRequest<bool>
{
    public int Id { get; set; }

    public DeleteStudentCommand(int id)
    {
        Id = id;
    }
}