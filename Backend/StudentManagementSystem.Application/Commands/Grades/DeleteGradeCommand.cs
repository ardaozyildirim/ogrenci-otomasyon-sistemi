using MediatR;

namespace StudentManagementSystem.Application.Commands.Grades;

public class DeleteGradeCommand : IRequest<Unit>
{
    public int Id { get; }

    public DeleteGradeCommand(int id)
    {
        Id = id;
    }
}