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
        if (string.IsNullOrWhiteSpace(employeeNumber))
            throw new ArgumentException("Employee number cannot be null or empty", nameof(employeeNumber));

        if (!EmployeeNumberRegex.IsMatch(employeeNumber))
            throw new ArgumentException("Employee number must be in format: EMPXXXX (e.g., EMP0001)", nameof(employeeNumber));

        return new EmployeeNumber(employeeNumber.ToUpperInvariant());
    }

    public static implicit operator string(EmployeeNumber employeeNumber) => employeeNumber.Value;

    public override string ToString() => Value;
}
