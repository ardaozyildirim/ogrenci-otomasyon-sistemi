using System.Text.RegularExpressions;

namespace StudentManagementSystem.Domain.ValueObjects;

public record CourseCode
{
    private static readonly Regex CourseCodeRegex = new(
        @"^[A-Z]{2,4}[0-9]{3}$",
        RegexOptions.Compiled);

    public string Value { get; }

    private CourseCode(string value)
    {
        Value = value;
    }

    public static CourseCode Create(string courseCode)
    {
        if (string.IsNullOrWhiteSpace(courseCode))
            throw new ArgumentException("Course code cannot be null or empty", nameof(courseCode));

        if (!CourseCodeRegex.IsMatch(courseCode))
            throw new ArgumentException("Course code must be in format: AAAAXXX (e.g., CS101, MATH201)", nameof(courseCode));

        return new CourseCode(courseCode.ToUpperInvariant());
    }

    public static implicit operator string(CourseCode courseCode) => courseCode.Value;

    public override string ToString() => Value;
}
