using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Create_ValidEmail_ShouldReturnEmail()
    {
        // Arrange
        var validEmail = "test@example.com";

        // Act
        var email = Email.Create(validEmail);

        // Assert
        Assert.Equal("test@example.com", email.Value);
    }

    [Fact]
    public void Create_ValidEmailWithUpperCase_ShouldConvertToLowerCase()
    {
        // Arrange
        var validEmail = "TEST@EXAMPLE.COM";

        // Act
        var email = Email.Create(validEmail);

        // Assert
        Assert.Equal("test@example.com", email.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidEmail_ShouldThrowArgumentException(string invalidEmail)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Email.Create(invalidEmail));
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@example.com")]
    [InlineData("test@")]
    [InlineData("test.example.com")]
    public void Create_MalformedEmail_ShouldThrowArgumentException(string malformedEmail)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Email.Create(malformedEmail));
    }

    [Fact]
    public void ImplicitConversion_ShouldWork()
    {
        // Arrange
        var email = Email.Create("test@example.com");

        // Act
        string emailString = email;

        // Assert
        Assert.Equal("test@example.com", emailString);
    }
}
