using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Services;

public class StudentNumberGenerator : IStudentNumberGenerator
{
    public string GenerateStudentNumber(string department, int year)
    {
        if (string.IsNullOrWhiteSpace(department))
            throw new ArgumentException("Department cannot be null or empty", nameof(department));

        if (year < 2000 || year > DateTime.Now.Year + 1)
            throw new ArgumentException("Invalid year", nameof(year));

        var departmentCode = department.ToUpperInvariant()[..2];
        var yearString = year.ToString();
        var randomNumber = Random.Shared.Next(1, 1000).ToString("D3");

        var studentNumber = $"{yearString}{departmentCode}{randomNumber}";
        
        return StudentNumber.Create(studentNumber).Value;
    }
}
