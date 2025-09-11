using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Tests.ValueObjects;

public class StudentNumberTests
{
    [Fact]
    public void CreateStudentNumber_WithValidFormat_Works()
    {
        // Use a realistic student number
        var validNumber = "2023CS157";

        // Create the student number
        var studentNumber = StudentNumber.Create(validNumber);

        // Should match exactly
        Assert.Equal("2023CS157", studentNumber.Value);
    }

    [Fact]
    public void CreateStudentNumber_WithLowercase_ConvertsToUppercase()
    {
        // Try with lowercase department code
        var lowercaseNumber = "2022ee089";

        // Should automatically convert to uppercase
        var studentNumber = StudentNumber.Create(lowercaseNumber);

        // Verify conversion happened
        Assert.Equal("2022EE089", studentNumber.Value);
    }

    [Fact]
    public void CreateStudentNumber_WithEmptyInput_ThrowsException()
    {
        // These should all fail
        Assert.Throws<ArgumentException>(() => StudentNumber.Create(""));
        Assert.Throws<ArgumentException>(() => StudentNumber.Create(null!));
    }

    [Fact]
    public void CreateStudentNumber_WithWrongFormat_ThrowsException()
    {
        // Try with obviously wrong format - too short
        var badNumber = "2023CS";

        // Should reject invalid format
        Assert.Throws<ArgumentException>(() => StudentNumber.Create(badNumber));
    }

    [Fact]
    public void ToString_ReturnsTheActualValue()
    {
        // Create a student number
        var studentNumber = StudentNumber.Create("2021IT234");

        // ToString should give us the value back
        var stringValue = studentNumber.ToString();
        Assert.Equal("2021IT234", stringValue);
    }
}
