using FluentValidation.TestHelper;
using StudentManagementSystem.Application.Commands.Auth;
using StudentManagementSystem.Application.Validators;

namespace StudentManagementSystem.Application.Tests.Validators;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator;

    public LoginCommandValidatorTests()
    {
        _validator = new LoginCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "john.doe@example.com",
            Password = "password123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-email")]
    [InlineData("@example.com")]
    [InlineData("john@")]
    [InlineData("john.doe")]
    public void Validate_InvalidEmail_ShouldHaveValidationError(string email)
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = email,
            Password = "password123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_NullEmail_ShouldHaveValidationError()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = null!,
            Password = "password123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("john.doe@example.com")]
    [InlineData("user@domain.co.uk")]
    [InlineData("test.email@subdomain.example.org")]
    [InlineData("user+tag@example.com")]
    public void Validate_ValidEmail_ShouldNotHaveValidationError(string email)
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = email,
            Password = "password123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("123")]
    [InlineData("pass")]
    public void Validate_InvalidPassword_ShouldHaveValidationError(string password)
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "john.doe@example.com",
            Password = password
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_NullPassword_ShouldHaveValidationError()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "john.doe@example.com",
            Password = null!
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("password123")]
    [InlineData("securePassword")]
    [InlineData("MySecure123")]
    [InlineData("12345678")]
    public void Validate_ValidPassword_ShouldNotHaveValidationError(string password)
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "john.doe@example.com",
            Password = password
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
}
