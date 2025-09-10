using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Tests.ValueObjects;

public class CourseCodeTests
{
    [Theory]
    [InlineData("CS101")]
    [InlineData("MATH201")]
    [InlineData("ENG301")]
    [InlineData("PHYS401")]
    public void Create_ValidCourseCode_ShouldCreateSuccessfully(string value)
    {
        // Act
        var courseCode = CourseCode.Create(value);

        // Assert
        Assert.Equal(value, courseCode.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("CS")]
    [InlineData("CS10")]
    [InlineData("CS1234")]
    [InlineData("cs101")]
    [InlineData("101CS")]
    [InlineData("CS-101")]
    [InlineData("CS 101")]
    public void Create_InvalidCourseCode_ShouldThrowArgumentException(string value)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => CourseCode.Create(value));
    }

    [Fact]
    public void Equals_SameValue_ShouldReturnTrue()
    {
        // Arrange
        var courseCode1 = CourseCode.Create("CS101");
        var courseCode2 = CourseCode.Create("CS101");

        // Act & Assert
        Assert.True(courseCode1.Equals(courseCode2));
        Assert.True(courseCode1 == courseCode2);
    }

    [Fact]
    public void Equals_DifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var courseCode1 = CourseCode.Create("CS101");
        var courseCode2 = CourseCode.Create("CS102");

        // Act & Assert
        Assert.False(courseCode1.Equals(courseCode2));
        Assert.False(courseCode1 == courseCode2);
    }

    [Fact]
    public void GetHashCode_SameValue_ShouldReturnSameHashCode()
    {
        // Arrange
        var courseCode1 = CourseCode.Create("CS101");
        var courseCode2 = CourseCode.Create("CS101");

        // Act & Assert
        Assert.Equal(courseCode1.GetHashCode(), courseCode2.GetHashCode());
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var courseCode = CourseCode.Create("CS101");

        // Act
        var result = courseCode.ToString();

        // Assert
        Assert.Equal("CS101", result);
    }
}
