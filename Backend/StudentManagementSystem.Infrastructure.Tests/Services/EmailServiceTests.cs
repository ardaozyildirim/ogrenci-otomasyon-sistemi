using Microsoft.Extensions.Logging;
using Moq;
using StudentManagementSystem.Infrastructure.Services;

namespace StudentManagementSystem.Infrastructure.Tests.Services;

public class EmailServiceTests
{
    private readonly Mock<ILogger<EmailService>> _loggerMock;
    private readonly EmailService _emailService;

    public EmailServiceTests()
    {
        _loggerMock = new Mock<ILogger<EmailService>>();
        _emailService = new EmailService(_loggerMock.Object);
    }

    [Fact]
    public async Task SendEmailAsync_ValidEmail_ShouldCompleteSuccessfully()
    {
        // Arrange
        var to = "test@example.com";
        var subject = "Test Subject";
        var body = "Test Body";

        // Act
        await _emailService.SendEmailAsync(to, subject, body);

        // Assert
        // Verify that the method completes without throwing an exception
        Assert.True(true);
    }

    [Fact]
    public async Task SendWelcomeEmailAsync_ValidData_ShouldCompleteSuccessfully()
    {
        // Arrange
        var to = "newuser@example.com";
        var name = "John Doe";

        // Act
        await _emailService.SendWelcomeEmailAsync(to, name);

        // Assert
        // Verify that the method completes without throwing an exception
        Assert.True(true);
    }

    [Fact]
    public async Task SendGradeNotificationEmailAsync_ValidData_ShouldCompleteSuccessfully()
    {
        // Arrange
        var to = "student@example.com";
        var studentName = "Jane Smith";
        var courseName = "Data Structures";
        var score = 85.5m;

        // Act
        await _emailService.SendGradeNotificationEmailAsync(to, studentName, courseName, score);

        // Assert
        // Verify that the method completes without throwing an exception
        Assert.True(true);
    }
}
