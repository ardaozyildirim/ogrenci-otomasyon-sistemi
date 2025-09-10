using Moq;
using StudentManagementSystem.Application.Commands.Courses;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Tests.Commands;

public class CreateCourseCommandTests
{
    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateCourse()
    {
        // Arrange
        var mockCourseRepository = new Mock<ICourseRepository>();
        var mockTeacherRepository = new Mock<ITeacherRepository>();
        
        var teacher = new Teacher
        {
            Id = 1,
            UserId = 1,
            EmployeeNumber = "EMP001",
            Department = "Computer Science"
        };

        var course = new Course
        {
            Id = 1,
            Name = "Data Structures",
            Code = "CS101",
            Credits = 3,
            TeacherId = 1,
            Description = "Introduction to data structures"
        };

        mockTeacherRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(teacher);

        mockCourseRepository.Setup(x => x.AddAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(course);

        var command = new CreateCourseCommand
        {
            Name = "Data Structures",
            Code = "CS101",
            Credits = 3,
            TeacherId = 1,
            Description = "Introduction to data structures",
            Schedule = "Mon, Wed, Fri 10:00-11:00",
            Location = "Room 101"
        };

        var handler = new CreateCourseCommandHandler(mockCourseRepository.Object, mockTeacherRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(1, result);
        mockTeacherRepository.Verify(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        mockCourseRepository.Verify(x => x.AddAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_TeacherNotFound_ShouldThrowArgumentException()
    {
        // Arrange
        var mockCourseRepository = new Mock<ICourseRepository>();
        var mockTeacherRepository = new Mock<ITeacherRepository>();

        mockTeacherRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Teacher?)null);

        var command = new CreateCourseCommand
        {
            Name = "Data Structures",
            Code = "CS101",
            Credits = 3,
            TeacherId = 1
        };

        var handler = new CreateCourseCommandHandler(mockCourseRepository.Object, mockTeacherRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));
        mockCourseRepository.Verify(x => x.AddAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
