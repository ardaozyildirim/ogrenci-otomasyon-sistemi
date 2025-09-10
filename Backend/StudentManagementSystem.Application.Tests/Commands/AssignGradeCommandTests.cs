using Moq;
using StudentManagementSystem.Application.Commands.Grades;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Tests.Commands;

public class AssignGradeCommandTests
{
    private readonly Mock<IGradeRepository> _gradeRepositoryMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly AssignGradeCommandHandler _handler;

    public AssignGradeCommandTests()
    {
        _gradeRepositoryMock = new Mock<IGradeRepository>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _handler = new AssignGradeCommandHandler(_gradeRepositoryMock.Object, _studentRepositoryMock.Object, _courseRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldAssignGrade()
    {
        // Arrange
        var command = new AssignGradeCommand
        {
            StudentId = 1,
            CourseId = 1,
            Score = 85,
            GradeType = "Midterm",
            Comment = "Good work"
        };

        var student = Student.Create(1, "2024CS001", "Computer Science");
        var course = Course.Create("Data Structures", "CS101", 3, 1);
        var grade = Grade.Create(command.StudentId, command.CourseId, command.Score, command.GradeType, command.Comment);

        _studentRepositoryMock.Setup(x => x.GetByIdAsync(command.StudentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _courseRepositoryMock.Setup(x => x.GetByIdAsync(command.CourseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        _gradeRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Grade>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(grade);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(grade.Id, result);
        _gradeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Grade>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_StudentNotFound_ShouldThrowArgumentException()
    {
        // Arrange
        var command = new AssignGradeCommand
        {
            StudentId = 999,
            CourseId = 1,
            Score = 85
        };

        _studentRepositoryMock.Setup(x => x.GetByIdAsync(command.StudentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_CourseNotFound_ShouldThrowArgumentException()
    {
        // Arrange
        var command = new AssignGradeCommand
        {
            StudentId = 1,
            CourseId = 999,
            Score = 85
        };

        var student = Student.Create(1, "2024CS001", "Computer Science");

        _studentRepositoryMock.Setup(x => x.GetByIdAsync(command.StudentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        _courseRepositoryMock.Setup(x => x.GetByIdAsync(command.CourseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Course?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
