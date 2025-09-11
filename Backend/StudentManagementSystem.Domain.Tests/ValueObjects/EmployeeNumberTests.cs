using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Tests.ValueObjects;

public class EmployeeNumberTests
{
    [Fact]
    public void CreateEmployeeNumber_WithValidFormat_WorksCorrectly()
    {
        // Try with a valid employee number
        string validNumber = "EMP0142";

        // Create the employee number
        var employeeNumber = EmployeeNumber.Create(validNumber);

        // Should store the value correctly
        Assert.Equal(validNumber, employeeNumber.Value);
    }

    [Fact]
    public void CreateEmployeeNumber_WithBadFormat_ThrowsError()
    {
        // This format is missing a digit - should fail
        string invalidNumber = "EMP001";

        // Should reject the invalid format
        Assert.Throws<ArgumentException>(() => EmployeeNumber.Create(invalidNumber));
    }

    [Fact]
    public void CreateEmployeeNumber_WithNullOrEmpty_ThrowsError()
    {
        // Both empty and null should be rejected
        Assert.Throws<ArgumentException>(() => EmployeeNumber.Create(""));
        Assert.Throws<ArgumentException>(() => EmployeeNumber.Create(null!));
    }

    [Fact]
    public void ToString_GivesBackTheValue()
    {
        // Create an employee number
        var employeeNumber = EmployeeNumber.Create("EMP0987");

        // ToString should return the actual value
        string result = employeeNumber.ToString();
        Assert.Equal("EMP0987", result);
    }
}
