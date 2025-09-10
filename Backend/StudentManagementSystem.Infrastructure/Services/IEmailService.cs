namespace StudentManagementSystem.Infrastructure.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    Task SendWelcomeEmailAsync(string to, string name, CancellationToken cancellationToken = default);
    Task SendGradeNotificationEmailAsync(string to, string studentName, string courseName, decimal score, CancellationToken cancellationToken = default);
}
