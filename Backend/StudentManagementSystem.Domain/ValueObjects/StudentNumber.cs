using System.Text.RegularExpressions;

namespace StudentManagementSystem.Domain.ValueObjects;

public record StudentNumber
{
    private static readonly Regex StudentNumberRegex = new(
        @"^[0-9]{4}[A-Z]{2}[0-9]{3}$",
        RegexOptions.Compiled);

    public string Value { get; }

    private StudentNumber(string value)
    {
        Value = value;
    }

    public static StudentNumber Create(string studentNumber)
    {
        ValidateStudentNumber(studentNumber);
        
        var normalizedNumber = NormalizeStudentNumber(studentNumber);
        EnsureValidFormat(normalizedNumber);

        return new StudentNumber(normalizedNumber);
    }

    private static void ValidateStudentNumber(string studentNumber)
    {
        if (string.IsNullOrWhiteSpace(studentNumber))
            throw new ArgumentException("Student number is required", nameof(studentNumber));
    }

    private static string NormalizeStudentNumber(string studentNumber)
    {
        return studentNumber.ToUpperInvariant().Trim();
    }

    private static void EnsureValidFormat(string normalizedNumber)
    {
        if (!StudentNumberRegex.IsMatch(normalizedNumber))
            throw new ArgumentException("Student number must be in format: YYYYAAXXX (e.g., 2024CS001)");
    }

    public static implicit operator string(StudentNumber studentNumber) => studentNumber.Value;

    public override string ToString() => Value;
}
