using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Domain.Tests.Entities;

public class CourseTests
{
    [Fact]
    public void Create_ValidCourse_ShouldWork()
    {
        // Arrange
        string courseName = "Math 101";
        string courseCode = "MATH101";
        int credits = 3;
        int teacherId = 1;

        // Act
        var course = Course.Create(courseName, courseCode, credits, teacherId);

        // Assert
        Assert.Equal(courseName, course.Name);
        Assert.Equal(courseCode, course.Code);
        Assert.Equal(credits, course.Credits);
        Assert.Equal(teacherId, course.TeacherId);
        Assert.Equal(CourseStatus.NotStarted, course.Status);
    }

    [Fact]
    public void StartCourse_ShouldChangeStatus()
    {
        // Arrange
        var course = Course.Create("Math 101", "MATH101", 3, 1);

        // Act
        course.StartCourse();

        // Assert
        Assert.Equal(CourseStatus.InProgress, course.Status);
        Assert.NotNull(course.StartDate);
    }

    [Fact]
    public void CompleteCourse_ShouldChangeStatus()
    {
        // Arrange
        var course = Course.Create("Math 101", "MATH101", 3, 1);
        course.StartCourse();

        // Act
        course.CompleteCourse();

        // Assert
        Assert.Equal(CourseStatus.Completed, course.Status);
        Assert.NotNull(course.EndDate);
    }

    [Fact]
    public void EnrollStudent_ShouldWork()
    {
        // Arrange
        var course = Course.Create("Math 101", "MATH101", 3, 1);
        course.StartCourse();
        int studentId = 1;

        // Act
        course.EnrollStudent(studentId);

        // Assert
        Assert.Equal(1, course.GetEnrolledStudentCount());
    }

    [Fact]
    public void GetEnrolledStudentCount_ShouldReturnCorrectNumber()
    {
        // Arrange
        var course = Course.Create("Math 101", "MATH101", 3, 1);
        course.StartCourse();
        
        course.EnrollStudent(1);
        course.EnrollStudent(2);

        // Act
        int count = course.GetEnrolledStudentCount();

        // Assert
        Assert.Equal(2, count);
    }
}
