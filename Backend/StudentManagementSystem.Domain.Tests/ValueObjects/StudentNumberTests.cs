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

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidStudentNumber_ShouldThrowArgumentException(string invalidStudentNumber)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => StudentNumber.Create(invalidStudentNumber));
    }

    [Theory]
    [InlineData("2024CS")]
    [InlineData("24CS001")]
    [InlineData("2024C001")]
    [InlineData("2024CS01")]
    [InlineData("2024CS0001")]
    public void Create_MalformedStudentNumber_ShouldThrowArgumentException(string malformedStudentNumber)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => StudentNumber.Create(malformedStudentNumber));
    }

    [Fact]
    public void ImplicitConversion_ShouldWork()
    {
        // Arrange
        var studentNumber = StudentNumber.Create("2024CS001");

        // Act
        string studentNumberString = studentNumber;

        // Assert
        Assert.Equal("2024CS001", studentNumberString);
    }
}
