using MediatR;

namespace StudentManagementSystem.Application.Commands.Grades;

public class RestoreGradeCommand : IRequest<Unit>
{
    public int Id { get; }

    public RestoreGradeCommand(int id)
    {
        Id = id;
    }
}