namespace StudentManagementSystem.Domain.Services;

public interface IStudentNumberGenerator
{
    string GenerateStudentNumber(string department, int year);
}
