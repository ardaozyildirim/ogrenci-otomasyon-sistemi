using Moq;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.Queries.Students;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Tests.Queries;

public class GetStudentByIdQueryTests
{
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly GetStudentByIdQueryHandler _handler;

    public GetStudentByIdQueryTests()
    {
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _handler = new GetStudentByIdQueryHandler(_studentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidId_ShouldReturnStudent()
    {
        // Arrange
        var query = new GetStudentByIdQuery { Id = 1 };
        var student = Student.Create(1, "2024CS001", "Computer Science");

        _studentRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(student.Id, result.Id);
        Assert.Equal(student.StudentNumber, result.StudentNumber);
    }

    [Fact]
    public async Task Handle_InvalidId_ShouldReturnNull()
    {
        // Arrange
        var query = new GetStudentByIdQuery { Id = 999 };

        _studentRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
