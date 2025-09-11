using System.Text.RegularExpressions;

namespace StudentManagementSystem.Domain.ValueObjects;

public record EmployeeNumber
{
    private static readonly Regex EmployeeNumberRegex = new(
        @"^EMP[0-9]{4}$",
        RegexOptions.Compiled);

    public string Value { get; }

    private EmployeeNumber(string value)
    {
        Value = value;
    }

    public static EmployeeNumber Create(string employeeNumber)
    {
        ValidateEmployeeNumber(employeeNumber);
        
        var normalizedNumber = NormalizeEmployeeNumber(employeeNumber);
        EnsureValidFormat(normalizedNumber);

        return new EmployeeNumber(normalizedNumber);
    }

    private static void ValidateEmployeeNumber(string employeeNumber)
    {
        if (string.IsNullOrWhiteSpace(employeeNumber))
            throw new ArgumentException("Employee number is required", nameof(employeeNumber));
    }

    private static string NormalizeEmployeeNumber(string employeeNumber)
    {
        return employeeNumber.ToUpperInvariant().Trim();
    }

    private static void EnsureValidFormat(string normalizedNumber)
    {
        if (!EmployeeNumberRegex.IsMatch(normalizedNumber))
            throw new ArgumentException("Employee number must be in format: EMPXXXX (e.g., EMP0001)");
    }

    public static implicit operator string(EmployeeNumber employeeNumber) => employeeNumber.Value;

    public override string ToString() => Value;
}
