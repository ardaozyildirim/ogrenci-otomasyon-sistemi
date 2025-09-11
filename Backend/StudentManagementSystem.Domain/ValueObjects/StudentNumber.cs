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
        if (string.IsNullOrWhiteSpace(studentNumber))
            throw new ArgumentException("Student number cannot be null or empty", nameof(studentNumber));

        var upperStudentNumber = studentNumber.ToUpperInvariant();
        
        if (!StudentNumberRegex.IsMatch(upperStudentNumber))
            throw new ArgumentException("Student number must be in format: YYYYAAXXX (e.g., 2024CS001)", nameof(studentNumber));

        return new StudentNumber(upperStudentNumber);
    }

    public static implicit operator string(StudentNumber studentNumber) => studentNumber.Value;

    public override string ToString() => Value;
}
