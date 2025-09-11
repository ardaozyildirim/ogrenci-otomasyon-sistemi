using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Domain.Tests.Entities;

public class CourseTests
{
    [Fact]
    public void CreateCourse_WithValidData_ReturnsCorrectCourse()
    {
        // Given
        var name = "Introduction to Computer Science";
        var code = "CS101";
        var creditHours = 4;
        var instructorId = 5;

        // When
        var newCourse = Course.Create(name, code, creditHours, instructorId);

        // Then
        Assert.Equal(name, newCourse.Name);
        Assert.Equal(code, newCourse.Code);
        Assert.Equal(creditHours, newCourse.Credits);
        Assert.Equal(instructorId, newCourse.TeacherId);
        Assert.Equal(CourseStatus.NotStarted, newCourse.Status);
    }

    [Fact]
    public void StartCourse_ChangesStatusToInProgress()
    {
        // Setup a new course first
        var physics = Course.Create("Physics 201", "PHYS201", 3, 12);

        // Start the course
        physics.StartCourse();

        // Check if status changed and start date is set
        Assert.Equal(CourseStatus.InProgress, physics.Status);
        Assert.True(physics.StartDate.HasValue);
        Assert.True(physics.StartDate.Value <= DateTime.Now);
    }

    [Fact]
    public void CompleteCourse_WhenStarted_MarksAsCompleted()
    {
        // Create and start a course
        var biology = Course.Create("Biology Fundamentals", "BIO101", 3, 8);
        biology.StartCourse();

        // Complete the course
        biology.CompleteCourse();

        // Verify completion
        Assert.Equal(CourseStatus.Completed, biology.Status);
        Assert.NotNull(biology.EndDate);
        Assert.True(biology.EndDate!.Value >= biology.StartDate!.Value);
    }

    [Fact]
    public void EnrollStudent_InActiveCourse_AddsStudentSuccessfully()
    {
        // Prepare an active course
        var chemistry = Course.Create("Organic Chemistry", "CHEM301", 4, 15);
        chemistry.StartCourse();
        var studentId = 42;

        // Enroll the student
        chemistry.EnrollStudent(studentId);

        // Confirm enrollment
        var enrolledCount = chemistry.GetEnrolledStudentCount();
        Assert.Equal(1, enrolledCount);
    }

    [Fact]
    public void GetEnrolledStudentCount_WithMultipleStudents_ReturnsCorrectNumber()
    {
        // Set up course and get it running
        var history = Course.Create("World History", "HIST102", 3, 7);
        history.StartCourse();
        
        // Add a few students
        history.EnrollStudent(101);
        history.EnrollStudent(205);
        history.EnrollStudent(333);

        // Check the count
        var totalEnrolled = history.GetEnrolledStudentCount();
        Assert.Equal(3, totalEnrolled);
    }

    [Fact]
    public void CourseCreation_SetsInitialStatusCorrectly()
    {
        // Create any course
        var literature = Course.Create("English Literature", "ENG201", 3, 22);
        
        // New courses should start in NotStarted status
        Assert.Equal(CourseStatus.NotStarted, literature.Status);
        Assert.Null(literature.StartDate);
        Assert.Null(literature.EndDate);
    }
}
