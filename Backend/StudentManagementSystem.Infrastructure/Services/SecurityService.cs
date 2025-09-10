using System.Text.RegularExpressions;

namespace StudentManagementSystem.Infrastructure.Services;

public class SecurityService : ISecurityService
{
    private static readonly string[] SqlInjectionPatterns = {
        @"(\b(SELECT|INSERT|UPDATE|DELETE|DROP|CREATE|ALTER|EXEC|UNION|SCRIPT)\b)",
        @"(\b(OR|AND)\s+\d+\s*=\s*\d+)",
        @"(\b(OR|AND)\s+'.*'\s*=\s*'.*')",
        @"(;|\-\-|\/\*|\*\/)",
        @"(\bUNION\s+SELECT\b)",
        @"(\bDROP\s+TABLE\b)",
        @"(\bINSERT\s+INTO\b)",
        @"(\bDELETE\s+FROM\b)",
        @"(\bUPDATE\s+.*\s+SET\b)",
        @"(\bEXEC\s*\()",
        @"(\bSCRIPT\b)"
    };

    public string SanitizeInput(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Remove potentially dangerous characters
        input = Regex.Replace(input, @"[<>""'%;()&+]", string.Empty);
        
        // Trim whitespace
        input = input.Trim();
        
        return input;
    }

    public bool IsValidInput(string input)
    {
        if (string.IsNullOrEmpty(input))
            return true;

        // Check for SQL injection patterns
        if (ContainsSqlInjectionPattern(input))
            return false;

        // Check for XSS patterns
        if (ContainsXssPattern(input))
            return false;

        return true;
    }

    public string EscapeHtml(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        return input
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#x27;")
            .Replace("/", "&#x2F;");
    }

    public bool ContainsSqlInjectionPattern(string input)
    {
        if (string.IsNullOrEmpty(input))
            return false;

        var upperInput = input.ToUpperInvariant();

        foreach (var pattern in SqlInjectionPatterns)
        {
            if (Regex.IsMatch(upperInput, pattern, RegexOptions.IgnoreCase))
                return true;
        }

        return false;
    }

    private static bool ContainsXssPattern(string input)
    {
        var xssPatterns = new[]
        {
            @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>",
            @"javascript:",
            @"on\w+\s*=",
            @"<iframe\b",
            @"<object\b",
            @"<embed\b",
            @"<link\b",
            @"<meta\b"
        };

        foreach (var pattern in xssPatterns)
        {
            if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
                return true;
        }

        return false;
    }
}
