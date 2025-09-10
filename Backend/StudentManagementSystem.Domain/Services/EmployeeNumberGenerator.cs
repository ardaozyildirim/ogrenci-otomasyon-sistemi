using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Services;

public class EmployeeNumberGenerator : IEmployeeNumberGenerator
{
    public string GenerateEmployeeNumber()
    {
        var randomNumber = Random.Shared.Next(1, 10000).ToString("D4");
        var employeeNumber = $"EMP{randomNumber}";
        
        return EmployeeNumber.Create(employeeNumber).Value;
    }
}
