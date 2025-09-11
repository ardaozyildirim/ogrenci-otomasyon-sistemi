using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Services;

namespace StudentManagementSystem.Domain.Tests.Services;

public class GradeCalculationServiceTests
{

    [Fact]
    public void CalculateAverageGrade_WithThreeGrades_GetsCorrectResult()
    {
        // Set up the service and some test grades
        var calculator = new GradeCalculationService();
        var studentGrades = new List<Grade>
        {
            CreateGrade(82),
            CreateGrade(91),
            CreateGrade(67)
        };

        // Calculate the average
        var averageScore = calculator.CalculateAverageGrade(studentGrades);

        // Should be 80 (82+91+67)/3 = 240/3 = 80
        Assert.Equal(80, averageScore);
    }

    [Fact]
    public void CalculateAverageGrade_WithNoGrades_ReturnsZero()
    {
        // Create service with empty grade list
        var calculator = new GradeCalculationService();
        var emptyGradeList = new List<Grade>();

        // Calculate average of nothing
        var result = calculator.CalculateAverageGrade(emptyGradeList);

        // Should be zero when no grades exist
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetLetterGrade_WithExcellentScore_ReturnsA()
    {
        // Test with a score that should definitely be an A
        var gradeService = new GradeCalculationService();
        
        // 97 is clearly an A grade
        string letter = gradeService.GetLetterGrade(97);
        
        Assert.Equal("A", letter);
    }
    
    [Fact]
    public void GetLetterGrade_ScoreBetween80And89_ShouldReturnB()
    {
        // Arrange
        var service = new GradeCalculationService();
        
        // Act
        string letterGrade = service.GetLetterGrade(85);
        
        // Assert
        Assert.Equal("B", letterGrade);
    }
    
    [Fact]
    public void GetLetterGrade_ScoreBelow60_ShouldReturnF()
    {
        // Arrange
        var service = new GradeCalculationService();
        
        // Act
        string letterGrade = service.GetLetterGrade(55);
        
        // Assert
        Assert.Equal("F", letterGrade);
    }

    [Fact]
    public void IsPassingGrade_Score70_ShouldReturnTrue()
    {
        // Arrange
        var service = new GradeCalculationService();
        
        // Act
        bool isPassing = service.IsPassingGrade(70);
        
        // Assert
        Assert.True(isPassing);
    }
    
    [Fact]
    public void IsPassingGrade_Score59_ShouldReturnFalse()
    {
        // Arrange
        var service = new GradeCalculationService();
        
        // Act
        bool isPassing = service.IsPassingGrade(59);
        
        // Assert
        Assert.False(isPassing);
    }

    private static Grade CreateGrade(decimal score)
    {
        return new Grade
        {
            Score = score,
            Course = Course.Create("Test Course", "TEST101", 3, 1)
        };
    }
}
