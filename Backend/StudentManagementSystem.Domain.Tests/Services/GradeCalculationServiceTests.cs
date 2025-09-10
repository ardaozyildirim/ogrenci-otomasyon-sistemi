using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Services;

namespace StudentManagementSystem.Domain.Tests.Services;

public class GradeCalculationServiceTests
{
    private readonly IGradeCalculationService _gradeCalculationService;

    public GradeCalculationServiceTests()
    {
        _gradeCalculationService = new GradeCalculationService();
    }

    [Fact]
    public void CalculateAverageGrade_WithGrades_ShouldReturnCorrectAverage()
    {
        // Arrange
        var grades = new List<Grade>
        {
            CreateGrade(80),
            CreateGrade(90),
            CreateGrade(70)
        };

        // Act
        var average = _gradeCalculationService.CalculateAverageGrade(grades);

        // Assert
        Assert.Equal(80, average);
    }

    [Fact]
    public void CalculateAverageGrade_EmptyGrades_ShouldReturnZero()
    {
        // Arrange
        var grades = new List<Grade>();

        // Act
        var average = _gradeCalculationService.CalculateAverageGrade(grades);

        // Assert
        Assert.Equal(0, average);
    }

    [Fact]
    public void GetLetterGrade_ValidScores_ShouldReturnCorrectLetterGrade()
    {
        // Arrange & Act & Assert
        Assert.Equal("A", _gradeCalculationService.GetLetterGrade(95));
        Assert.Equal("B", _gradeCalculationService.GetLetterGrade(85));
        Assert.Equal("C", _gradeCalculationService.GetLetterGrade(75));
        Assert.Equal("D", _gradeCalculationService.GetLetterGrade(65));
        Assert.Equal("F", _gradeCalculationService.GetLetterGrade(55));
    }

    [Fact]
    public void IsPassingGrade_ValidScores_ShouldReturnCorrectResult()
    {
        // Arrange & Act & Assert
        Assert.True(_gradeCalculationService.IsPassingGrade(70));
        Assert.True(_gradeCalculationService.IsPassingGrade(60));
        Assert.False(_gradeCalculationService.IsPassingGrade(59));
        Assert.False(_gradeCalculationService.IsPassingGrade(0));
    }

    private static Grade CreateGrade(decimal score)
    {
        return new Grade
        {
            Score = score,
            Course = new Course { Credits = 3 }
        };
    }
}
