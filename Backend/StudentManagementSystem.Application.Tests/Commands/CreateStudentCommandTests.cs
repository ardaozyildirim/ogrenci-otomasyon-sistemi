using Moq;
using StudentManagementSystem.Application.Commands.Students;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Tests.Commands;

public class CreateStudentCommandTests
{
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly CreateStudentCommandHandler _handler;

    public CreateStudentCommandTests()
    {
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new CreateStudentCommandHandler(_studentRepositoryMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateStudent()
    {
        // Arrange
        var command = new CreateStudentCommand
        {
            UserId = 1,
            StudentNumber = "2024CS001",
            Department = "Computer Science",
            Grade = 1,
            ClassName = "CS-1A"
        };

        var user = User.Create("John", "Doe", "john@example.com", "password", UserRole.Student);
        var student = Student.Create(command.UserId, command.StudentNumber, command.Department, command.Grade, command.ClassName);

        _userRepositoryMock.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _studentRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(student.Id, result);
        _studentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldThrowArgumentException()
    {
        // Arrange
        var command = new CreateStudentCommand
        {
            UserId = 999,
            StudentNumber = "2024CS001"
        };

        _userRepositoryMock.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
