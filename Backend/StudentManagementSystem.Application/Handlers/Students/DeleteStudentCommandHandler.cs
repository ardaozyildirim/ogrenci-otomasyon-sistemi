using MediatR;
using StudentManagementSystem.Application.Commands.Students;
using StudentManagementSystem.Application.Common.Interfaces;

namespace StudentManagementSystem.Application.Handlers.Students;

public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, bool>
{
    private readonly IStudentRepository _studentRepository;

    public DeleteStudentCommandHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<bool> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.Id);
        if (student == null)
        {
            throw new KeyNotFoundException($"Student with ID {request.Id} not found.");
        }

        await _studentRepository.DeleteAsync(student);
        return true;
    }
}