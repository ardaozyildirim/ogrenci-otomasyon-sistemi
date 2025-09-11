using MediatR;
using StudentManagementSystem.Application.Commands.Teachers;
using StudentManagementSystem.Application.Common.Interfaces;

namespace StudentManagementSystem.Application.Handlers.Teachers;

public class DeleteTeacherCommandHandler : IRequestHandler<DeleteTeacherCommand, bool>
{
    private readonly ITeacherRepository _teacherRepository;

    public DeleteTeacherCommandHandler(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<bool> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository.GetByIdAsync(request.Id);
        if (teacher == null)
        {
            throw new InvalidOperationException("Teacher not found.");
        }

        await _teacherRepository.DeleteAsync(teacher);
        return true;
    }
}