using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Tests.ValueObjects;

public class EmployeeNumberTests
{
    [Fact]
    public void Create_ValidEmployeeNumber_ShouldWork()
    {
        // Arrange
        string validNumber = "EMP0001";

        // Act
        var employeeNumber = EmployeeNumber.Create(validNumber);

        // Assert
        Assert.Equal(validNumber, employeeNumber.Value);
    }

    [Fact]
    public void Create_InvalidEmployeeNumber_ShouldThrowError()
    {
        // Arrange
        string invalidNumber = "EMP001"; // Missing one digit

        // Act & Assert
        Assert.Throws<ArgumentException>(() => EmployeeNumber.Create(invalidNumber));
    }

    [Fact]
    public void Create_EmptyEmployeeNumber_ShouldThrowError()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => EmployeeNumber.Create(""));
        Assert.Throws<ArgumentException>(() => EmployeeNumber.Create(null!));
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var employeeNumber = EmployeeNumber.Create("EMP0001");

        // Act
        string result = employeeNumber.ToString();

        // Assert
        Assert.Equal("EMP0001", result);
    }
}
