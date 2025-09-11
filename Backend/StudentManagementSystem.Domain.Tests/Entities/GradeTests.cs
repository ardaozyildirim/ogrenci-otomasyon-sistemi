using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Domain.Tests.Entities;

public class GradeTests
{
    [Fact]
    public void Create_ValidGrade_ShouldWork()
    {
        // Arrange
        int studentId = 1;
        int courseId = 1;
        decimal score = 85;
        string gradeType = "Midterm";

        // Act
        var grade = Grade.Create(studentId, courseId, score, gradeType);

        // Assert
        Assert.Equal(studentId, grade.StudentId);
        Assert.Equal(courseId, grade.CourseId);
        Assert.Equal(score, grade.Score);
        Assert.Equal(gradeType, grade.GradeType);
    }

    [Fact]
    public void UpdateScore_ShouldChangeScore()
    {
        // Arrange
        var grade = Grade.Create(1, 1, 75, "Midterm");
        decimal newScore = 90;

        // Act
        grade.UpdateScore(newScore);

        // Assert
        Assert.Equal(newScore, grade.Score);
    }

    [Fact]
    public void GetLetterGrade_ShouldReturnCorrectLetter()
    {
        // Arrange
        var gradeA = Grade.Create(1, 1, 95, "Final");
        var gradeB = Grade.Create(1, 1, 85, "Final");
        var gradeF = Grade.Create(1, 1, 45, "Final");

        // Act & Assert
        Assert.Equal("A", gradeA.GetLetterGrade());
        Assert.Equal("B", gradeB.GetLetterGrade());
        Assert.Equal("F", gradeF.GetLetterGrade());
    }

    [Fact]
    public void IsPassingGrade_ShouldReturnCorrectResult()
    {
        // Arrange
        var passingGrade = Grade.Create(1, 1, 70, "Final");
        var failingGrade = Grade.Create(1, 1, 50, "Final");

        // Act & Assert
        Assert.True(passingGrade.IsPassingGrade());
        Assert.False(failingGrade.IsPassingGrade());
    }

    [Fact]
    public void Create_InvalidScore_ShouldThrowError()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Grade.Create(1, 1, -10, "Test"));
        Assert.Throws<ArgumentException>(() => Grade.Create(1, 1, 110, "Test"));
    }
}
