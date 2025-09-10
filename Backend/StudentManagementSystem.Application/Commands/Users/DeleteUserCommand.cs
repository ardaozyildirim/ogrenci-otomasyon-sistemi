using StudentManagementSystem.Application.Common.Interfaces;

namespace StudentManagementSystem.Application.Commands.Users;

public record DeleteUserCommand : ICommand
{
    public int Id { get; init; }
}

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
            throw new ArgumentException("User not found", nameof(request.Id));

        await _userRepository.DeleteAsync(user, cancellationToken);
    }
}
