using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Tests.ValueObjects;

public class StudentNumberTests
{
    [Fact]
    public void Create_ValidStudentNumber_ShouldReturnStudentNumber()
    {
        // Arrange
        var validStudentNumber = "2024CS001";

        // Act
        var studentNumber = StudentNumber.Create(validStudentNumber);

        // Assert
        Assert.Equal("2024CS001", studentNumber.Value);
    }

    [Fact]
    public void Create_ValidStudentNumberWithLowerCase_ShouldConvertToUpperCase()
    {
        // Arrange
        var validStudentNumber = "2024cs001";

        // Act
        var studentNumber = StudentNumber.Create(validStudentNumber);

        // Assert
        Assert.Equal("2024CS001", studentNumber.Value);
    }

    [Fact]
    public void Create_EmptyStudentNumber_ShouldThrowError()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => StudentNumber.Create(""));
        Assert.Throws<ArgumentException>(() => StudentNumber.Create(null!));
    }

    [Fact]
    public void Create_InvalidFormatStudentNumber_ShouldThrowError()
    {
        // Arrange
        string invalidNumber = "2024CS"; // Too short

        // Act & Assert
        Assert.Throws<ArgumentException>(() => StudentNumber.Create(invalidNumber));
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var studentNumber = StudentNumber.Create("2024CS001");

        // Act
        string result = studentNumber.ToString();

        // Assert
        Assert.Equal("2024CS001", result);
    }
}
