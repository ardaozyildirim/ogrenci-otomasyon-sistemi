using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Domain.Services;

public class GradeCalculationService : IGradeCalculationService
{
    public decimal CalculateAverageGrade(IEnumerable<Grade> grades)
    {
        var gradeList = grades.ToList();
        if (!gradeList.Any())
            return 0;

        return gradeList.Average(g => g.Score);
    }

    public decimal CalculateGPA(IEnumerable<Grade> grades, int totalCredits)
    {
        var gradeList = grades.ToList();
        if (!gradeList.Any() || totalCredits == 0)
            return 0;

        var totalPoints = gradeList.Sum(g => GetGradePoints(g.Score) * GetCourseCredits(g.Course));
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

    public bool IsPassingGrade(decimal score)
    {
        return score >= 60;
    }

    private static decimal GetGradePoints(decimal score)
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

    private static int GetCourseCredits(Course course)
    {
        return course?.Credits ?? 0;
    }
}
