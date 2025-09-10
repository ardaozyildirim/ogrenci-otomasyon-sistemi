using StudentManagementSystem.Infrastructure.Services;

namespace StudentManagementSystem.Infrastructure.Tests.Services;

public class PasswordHashServiceTests
{
    private readonly PasswordHashService _passwordHashService;

    public PasswordHashServiceTests()
    {
        _passwordHashService = new PasswordHashService();
    }

    [Fact]
    public void HashPassword_ValidPassword_ShouldReturnHashedPassword()
    {
        // Arrange
        var password = "TestPassword123";

        // Act
        var hashedPassword = _passwordHashService.HashPassword(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.NotEmpty(hashedPassword);
        Assert.NotEqual(password, hashedPassword);
    }

    [Fact]
    public void HashPassword_SamePassword_ShouldReturnDifferentHashes()
    {
        // Arrange
        var password = "TestPassword123";

        // Act
        var hash1 = _passwordHashService.HashPassword(password);
        var hash2 = _passwordHashService.HashPassword(password);

        // Assert
        Assert.NotEqual(hash1, hash2); // Different salts should produce different hashes
    }

    [Fact]
    public void VerifyPassword_ValidPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "TestPassword123";
        var hashedPassword = _passwordHashService.HashPassword(password);

        // Act
        var isValid = _passwordHashService.VerifyPassword(password, hashedPassword);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void VerifyPassword_InvalidPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "TestPassword123";
        var wrongPassword = "WrongPassword456";
        var hashedPassword = _passwordHashService.HashPassword(password);

        // Act
        var isValid = _passwordHashService.VerifyPassword(wrongPassword, hashedPassword);

        // Assert
        Assert.False(isValid);
    }

    [Theory]
    [InlineData("")]
    public void HashPassword_InvalidPassword_ShouldThrowArgumentException(string invalidPassword)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _passwordHashService.HashPassword(invalidPassword));
    }

    [Fact]
    public void HashPassword_NullPassword_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _passwordHashService.HashPassword(null!));
    }

    [Theory]
    [InlineData("", "validhash")]
    [InlineData("validpassword", "")]
    public void VerifyPassword_InvalidInput_ShouldReturnFalse(string password, string hashedPassword)
    {
        // Act
        var isValid = _passwordHashService.VerifyPassword(password, hashedPassword);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void VerifyPassword_NullPassword_ShouldReturnFalse()
    {
        // Act
        var isValid = _passwordHashService.VerifyPassword(null!, "validhash");

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void VerifyPassword_NullHashedPassword_ShouldReturnFalse()
    {
        // Act
        var isValid = _passwordHashService.VerifyPassword("validpassword", null!);

        // Assert
        Assert.False(isValid);
    }
}
