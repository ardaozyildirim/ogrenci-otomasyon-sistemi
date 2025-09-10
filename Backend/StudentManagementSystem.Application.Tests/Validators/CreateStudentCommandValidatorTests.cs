using FluentValidation.TestHelper;
using StudentManagementSystem.Application.Commands.Students;
using StudentManagementSystem.Application.Validators;

namespace StudentManagementSystem.Application.Tests.Validators;

public class CreateStudentCommandValidatorTests
{
    private readonly CreateStudentCommandValidator _validator;

    public CreateStudentCommandValidatorTests()
    {
        _validator = new CreateStudentCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateStudentCommand
        {
            UserId = 1,
            StudentNumber = "2024CS001",
            Department = "Computer Science",
            Grade = 10,
            ClassName = "A"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_UserIdZero_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateStudentCommand
        {
            UserId = 0,
            StudentNumber = "2024CS001",
            Department = "Computer Science"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("2024CS")]
    [InlineData("2024CS00123")]
    [InlineData("2024cs001")]
    [InlineData("2024-CS-001")]
    public void Validate_InvalidStudentNumber_ShouldHaveValidationError(string studentNumber)
    {
        // Arrange
        var command = new CreateStudentCommand
        {
            UserId = 1,
            StudentNumber = studentNumber,
            Department = "Computer Science"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StudentNumber);
    }

    [Fact]
    public void Validate_NullStudentNumber_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateStudentCommand
        {
            UserId = 1,
            StudentNumber = null!,
            Department = "Computer Science"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StudentNumber);
    }

    [Theory]
    [InlineData("Computer Science")]
    [InlineData("Mathematics")]
    [InlineData("Physics & Chemistry")]
    [InlineData("English Literature")]
    public void Validate_ValidDepartment_ShouldNotHaveValidationError(string department)
    {
        // Arrange
        var command = new CreateStudentCommand
        {
            UserId = 1,
            StudentNumber = "2024CS001",
            Department = department
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Department);
    }

    [Theory]
    [InlineData("Computer Science123")]
    [InlineData("Math@Science")]
    [InlineData("Physics#Chemistry")]
    [InlineData("English$Literature")]
    public void Validate_InvalidDepartment_ShouldHaveValidationError(string department)
    {
        // Arrange
        var command = new CreateStudentCommand
        {
            UserId = 1,
            StudentNumber = "2024CS001",
            Department = department
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Department);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    [InlineData(-1)]
    public void Validate_InvalidGrade_ShouldHaveValidationError(int grade)
    {
        // Arrange
        var command = new CreateStudentCommand
        {
            UserId = 1,
            StudentNumber = "2024CS001",
            Department = "Computer Science",
            Grade = grade
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Grade);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(6)]
    [InlineData(12)]
    public void Validate_ValidGrade_ShouldNotHaveValidationError(int grade)
    {
        // Arrange
        var command = new CreateStudentCommand
        {
            UserId = 1,
            StudentNumber = "2024CS001",
            Department = "Computer Science",
            Grade = grade
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Grade);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("10-A")]
    [InlineData("Science-1")]
    public void Validate_ValidClassName_ShouldNotHaveValidationError(string className)
    {
        // Arrange
        var command = new CreateStudentCommand
        {
            UserId = 1,
            StudentNumber = "2024CS001",
            Department = "Computer Science",
            ClassName = className
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ClassName);
    }

    [Theory]
    [InlineData("A@")]
    [InlineData("B#")]
    [InlineData("10-A$")]
    [InlineData("Science!1")]
    public void Validate_InvalidClassName_ShouldHaveValidationError(string className)
    {
        // Arrange
        var command = new CreateStudentCommand
        {
            UserId = 1,
            StudentNumber = "2024CS001",
            Department = "Computer Science",
            ClassName = className
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ClassName);
    }
}
