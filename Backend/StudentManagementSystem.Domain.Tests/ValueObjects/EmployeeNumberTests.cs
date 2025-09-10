using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Tests.ValueObjects;

public class EmployeeNumberTests
{
    [Theory]
    [InlineData("EMP001")]
    [InlineData("EMP999")]
    [InlineData("EMP1234")]
    public void Create_ValidEmployeeNumber_ShouldCreateSuccessfully(string value)
    {
        // Act
        var employeeNumber = EmployeeNumber.Create(value);

        // Assert
        Assert.Equal(value, employeeNumber.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("EMP")]
    [InlineData("EMP00")]
    [InlineData("EMP12345")]
    [InlineData("emp001")]
    [InlineData("123EMP")]
    [InlineData("EMP-001")]
    public void Create_InvalidEmployeeNumber_ShouldThrowArgumentException(string value)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => EmployeeNumber.Create(value));
    }

    [Fact]
    public void Equals_SameValue_ShouldReturnTrue()
    {
        // Arrange
        var employeeNumber1 = EmployeeNumber.Create("EMP001");
        var employeeNumber2 = EmployeeNumber.Create("EMP001");

        // Act & Assert
        Assert.True(employeeNumber1.Equals(employeeNumber2));
        Assert.True(employeeNumber1 == employeeNumber2);
    }

    [Fact]
    public void Equals_DifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var employeeNumber1 = EmployeeNumber.Create("EMP001");
        var employeeNumber2 = EmployeeNumber.Create("EMP002");

        // Act & Assert
        Assert.False(employeeNumber1.Equals(employeeNumber2));
        Assert.False(employeeNumber1 == employeeNumber2);
    }

    [Fact]
    public void GetHashCode_SameValue_ShouldReturnSameHashCode()
    {
        // Arrange
        var employeeNumber1 = EmployeeNumber.Create("EMP001");
        var employeeNumber2 = EmployeeNumber.Create("EMP001");

        // Act & Assert
        Assert.Equal(employeeNumber1.GetHashCode(), employeeNumber2.GetHashCode());
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var employeeNumber = EmployeeNumber.Create("EMP001");

        // Act
        var result = employeeNumber.ToString();

        // Assert
        Assert.Equal("EMP001", result);
    }
}
