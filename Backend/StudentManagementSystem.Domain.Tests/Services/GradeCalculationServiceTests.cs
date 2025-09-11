using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Services;

namespace StudentManagementSystem.Domain.Tests.Services;

public class GradeCalculationServiceTests
{

    [Fact]
    public void CalculateAverageGrade_ShouldReturnCorrectAverage()
    {
        // Arrange
        var service = new GradeCalculationService();
        var grades = new List<Grade>
        {
            CreateGrade(80),
            CreateGrade(90),
            CreateGrade(70)
        };

        // Act
        var average = service.CalculateAverageGrade(grades);

        // Assert
        Assert.Equal(80, average);
    }

    [Fact]
    public void CalculateAverageGrade_EmptyList_ShouldReturnZero()
    {
        // Arrange
        var service = new GradeCalculationService();
        var grades = new List<Grade>();

        // Act
        var average = service.CalculateAverageGrade(grades);

        // Assert
        Assert.Equal(0, average);
    }

    [Fact]
    public void GetLetterGrade_ScoreAbove90_ShouldReturnA()
    {
        // Arrange
        var service = new GradeCalculationService();
        
        // Act
        string letterGrade = service.GetLetterGrade(95);
        
        // Assert
        Assert.Equal("A", letterGrade);
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
            Course = new Course { Credits = 3 }
        };
    }
}
