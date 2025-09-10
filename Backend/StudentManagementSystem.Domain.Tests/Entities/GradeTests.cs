using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;
using StudentManagementSystem.Domain.Events.Grade;

namespace StudentManagementSystem.Domain.Tests.Entities;

public class GradeTests
{
    [Fact]
    public void Create_ValidGrade_ShouldSetPropertiesAndRaiseEvent()
    {
        // Arrange
        var studentId = 1;
        var courseId = 1;
        var score = 85;
        var studentName = "John Doe";
        var courseName = "Data Structures";

        // Act
        var grade = Grade.Create(studentId, courseId, score, studentName, courseName);

        // Assert
        Assert.Equal(studentId, grade.StudentId);
        Assert.Equal(courseId, grade.CourseId);
        Assert.Equal(score, grade.Score);
        Assert.Single(grade.DomainEvents);
        Assert.IsType<GradeAssignedEvent>(grade.DomainEvents.First());
    }

    [Fact]
    public void UpdateScore_ValidScore_ShouldUpdateScore()
    {
        // Arrange
        var grade = Grade.Create(1, 1, 75, "John Doe", "Data Structures");
        var newScore = 90;

        // Act
        grade.UpdateScore(newScore);

        // Assert
        Assert.Equal(newScore, grade.Score);
    }

    [Theory]
    [InlineData(97, LetterGrade.A_Plus)]
    [InlineData(93, LetterGrade.A)]
    [InlineData(90, LetterGrade.A_Minus)]
    [InlineData(87, LetterGrade.B_Plus)]
    [InlineData(83, LetterGrade.B)]
    [InlineData(80, LetterGrade.B_Minus)]
    [InlineData(77, LetterGrade.C_Plus)]
    [InlineData(73, LetterGrade.C)]
    [InlineData(70, LetterGrade.C_Minus)]
    [InlineData(67, LetterGrade.D_Plus)]
    [InlineData(63, LetterGrade.D)]
    [InlineData(60, LetterGrade.D_Minus)]
    [InlineData(59, LetterGrade.F)]
    public void GetLetterGrade_VariousScores_ShouldReturnCorrectLetterGrade(int score, LetterGrade expectedGrade)
    {
        // Arrange
        var grade = Grade.Create(1, 1, score, "John Doe", "Data Structures");

        // Act
        var letterGrade = grade.GetLetterGrade();

        // Assert
        Assert.Equal(expectedGrade, letterGrade);
    }

    [Theory]
    [InlineData(75, true)]
    [InlineData(60, true)]
    [InlineData(59, false)]
    [InlineData(0, false)]
    public void IsPassingGrade_VariousScores_ShouldReturnCorrectResult(int score, bool expectedResult)
    {
        // Arrange
        var grade = Grade.Create(1, 1, score, "John Doe", "Data Structures");

        // Act
        var isPassing = grade.IsPassingGrade();

        // Assert
        Assert.Equal(expectedResult, isPassing);
    }

    [Fact]
    public void Create_WithInvalidScore_ShouldThrowArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => Grade.Create(1, 1, -1, "John Doe", "Data Structures"));
        Assert.Throws<ArgumentException>(() => Grade.Create(1, 1, 101, "John Doe", "Data Structures"));
    }

    [Fact]
    public void UpdateScore_WithInvalidScore_ShouldThrowArgumentException()
    {
        // Arrange
        var grade = Grade.Create(1, 1, 75, "John Doe", "Data Structures");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => grade.UpdateScore(-1));
        Assert.Throws<ArgumentException>(() => grade.UpdateScore(101));
    }
}
