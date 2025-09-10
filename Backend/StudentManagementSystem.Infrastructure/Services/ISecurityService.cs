namespace StudentManagementSystem.Infrastructure.Services;

public interface ISecurityService
{
    string SanitizeInput(string input);
    bool IsValidInput(string input);
    string EscapeHtml(string input);
    bool ContainsSqlInjectionPattern(string input);
}
