using Moq;
using StudentManagementSystem.Application.Commands.Teachers;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Tests.Commands;

public class CreateTeacherCommandTests
{
    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateTeacher()
    {
        // Arrange
        var mockTeacherRepository = new Mock<ITeacherRepository>();
        var mockUserRepository = new Mock<IUserRepository>();
        
        var user = new User
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Role = UserRole.Teacher
        };

        var teacher = new Teacher
        {
            Id = 1,
            UserId = 1,
            EmployeeNumber = "EMP001",
            Department = "Computer Science",
            Specialization = "Software Engineering"
        };

        mockUserRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        mockTeacherRepository.Setup(x => x.AddAsync(It.IsAny<Teacher>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(teacher);

        var command = new CreateTeacherCommand
        {
            UserId = 1,
            EmployeeNumber = "EMP001",
            Department = "Computer Science",
            Specialization = "Software Engineering",
            HireDate = DateTime.Now
        };

        var handler = new CreateTeacherCommandHandler(mockTeacherRepository.Object, mockUserRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(1, result);
        mockUserRepository.Verify(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        mockTeacherRepository.Verify(x => x.AddAsync(It.IsAny<Teacher>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldThrowArgumentException()
    {
        // Arrange
        var mockTeacherRepository = new Mock<ITeacherRepository>();
        var mockUserRepository = new Mock<IUserRepository>();

        mockUserRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var command = new CreateTeacherCommand
        {
            UserId = 1,
            EmployeeNumber = "EMP001",
            Department = "Computer Science"
        };

        var handler = new CreateTeacherCommandHandler(mockTeacherRepository.Object, mockUserRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));
        mockTeacherRepository.Verify(x => x.AddAsync(It.IsAny<Teacher>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
