using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Domain.Services;

public class GradeCalculationService : IGradeCalculationService
{
    public decimal CalculateAverageGrade(IEnumerable<Grade> grades)
    {
        var gradesList = grades?.ToList() ?? new List<Grade>();
        
        return gradesList.Any() ? gradesList.Average(g => g.Score) : 0;
    }

    public decimal CalculateGPA(IEnumerable<Grade> grades, int totalCredits)
    {
        var gradesList = grades?.ToList() ?? new List<Grade>();
        
        if (!gradesList.Any() || totalCredits <= 0)
            return 0;

        var totalPoints = gradesList.Sum(g => ConvertToGradePoints(g.Score) * GetCreditsForCourse(g.Course));
        return totalPoints / totalCredits;
    }

    public string GetLetterGrade(decimal score)
    {
        return score switch
        {
            >= 90 => "A",
            >= 80 => "B", 
            >= 70 => "C",
            >= 60 => "D",
            _ => "F"
        };
    }

    public bool IsPassingGrade(decimal score) => score >= 60;

    private static decimal ConvertToGradePoints(decimal score)
    {
        return score switch
        {
            >= 90 => 4.0m,
            >= 80 => 3.0m,
            >= 70 => 2.0m,
            >= 60 => 1.0m,
            _ => 0.0m
        };
    }

    private static int GetCreditsForCourse(Course course)
    {
        return course?.Credits ?? 0;
    }
}
