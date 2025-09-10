using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Domain.Services;

public interface IGradeCalculationService
{
    decimal CalculateAverageGrade(IEnumerable<Grade> grades);
    decimal CalculateGPA(IEnumerable<Grade> grades, int totalCredits);
    string GetLetterGrade(decimal score);
    bool IsPassingGrade(decimal score);
}
