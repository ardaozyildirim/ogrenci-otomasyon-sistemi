using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;

namespace StudentManagementSystem.Application.Commands.Users;

public record UpdateUserCommand : ICommand<UserDto>
{
    public int Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhoneNumber { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public string? Address { get; init; }
}

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
            throw new ArgumentException($"User with ID {request.Id} not found", nameof(request.Id));

        user.UpdateProfile(
            request.FirstName ?? user.FirstName,
            request.LastName ?? user.LastName,
            request.PhoneNumber,
            request.DateOfBirth,
            request.Address);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return CreateUserDto(user);
    }

    private static UserDto CreateUserDto(Domain.Entities.User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Address = user.Address
        };
    }
}
