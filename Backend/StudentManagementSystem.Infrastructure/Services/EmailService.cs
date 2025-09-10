using Microsoft.Extensions.Logging;

namespace StudentManagementSystem.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending email to {Email} with subject: {Subject}", to, subject);
        
        // In a real application, this would integrate with an email service like SendGrid, AWS SES, etc.
        // For now, we'll just log the email details
        await Task.Delay(100, cancellationToken); // Simulate email sending
        
        _logger.LogInformation("Email sent successfully to {Email}", to);
    }

    public async Task SendWelcomeEmailAsync(string to, string name, CancellationToken cancellationToken = default)
    {
        var subject = "Welcome to Student Management System";
        var body = $@"
            <h1>Welcome {name}!</h1>
            <p>Your account has been created successfully in the Student Management System.</p>
            <p>You can now log in and start using the system.</p>
            <br>
            <p>Best regards,<br>Student Management System Team</p>
        ";

        await SendEmailAsync(to, subject, body, cancellationToken);
    }

    public async Task SendGradeNotificationEmailAsync(string to, string studentName, string courseName, decimal score, CancellationToken cancellationToken = default)
    {
        var subject = $"Grade Update - {courseName}";
        var body = $@"
            <h1>Grade Update Notification</h1>
            <p>Dear {studentName},</p>
            <p>Your grade for the course <strong>{courseName}</strong> has been updated.</p>
            <p><strong>Score: {score}/100</strong></p>
            <p>Please log in to the system to view more details.</p>
            <br>
            <p>Best regards,<br>Student Management System Team</p>
        ";

        await SendEmailAsync(to, subject, body, cancellationToken);
    }
}
