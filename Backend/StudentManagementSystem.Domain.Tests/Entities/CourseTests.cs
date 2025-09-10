using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;
using StudentManagementSystem.Domain.Events.Course;

namespace StudentManagementSystem.Domain.Tests.Entities;

public class CourseTests
{
    [Fact]
    public void Create_ValidCourse_ShouldSetProperties()
    {
        // Arrange
        var name = "Data Structures";
        var code = "CS101";
        var credits = 3;
        var teacherId = 1;
        var description = "Introduction to data structures";
        var schedule = "Mon, Wed, Fri 10:00-11:00";
        var location = "Room 101";

        // Act
        var course = Course.Create(name, code, credits, teacherId, description, schedule, location);

        // Assert
        Assert.Equal(name, course.Name);
        Assert.Equal(code, course.Code);
        Assert.Equal(credits, course.Credits);
        Assert.Equal(teacherId, course.TeacherId);
        Assert.Equal(description, course.Description);
        Assert.Equal(schedule, course.Schedule);
        Assert.Equal(location, course.Location);
        Assert.Equal(CourseStatus.Pending, course.Status);
    }

    [Fact]
    public void StartCourse_ShouldChangeStatusToActiveAndRaiseEvent()
    {
        // Arrange
        var course = Course.Create("Data Structures", "CS101", 3, 1, "Description", "Schedule", "Location");

        // Act
        course.StartCourse();

        // Assert
        Assert.Equal(CourseStatus.Active, course.Status);
        Assert.Single(course.DomainEvents);
        Assert.IsType<CourseStartedEvent>(course.DomainEvents.First());
    }

    [Fact]
    public void CompleteCourse_ShouldChangeStatusToCompleted()
    {
        // Arrange
        var course = Course.Create("Data Structures", "CS101", 3, 1, "Description", "Schedule", "Location");
        course.StartCourse(); // Start the course first

        // Act
        course.CompleteCourse();

        // Assert
        Assert.Equal(CourseStatus.Completed, course.Status);
    }

    [Fact]
    public void CancelCourse_ShouldChangeStatusToCancelled()
    {
        // Arrange
        var course = Course.Create("Data Structures", "CS101", 3, 1, "Description", "Schedule", "Location");

        // Act
        course.CancelCourse();

        // Assert
        Assert.Equal(CourseStatus.Cancelled, course.Status);
    }

    [Fact]
    public void EnrollStudent_ValidStudent_ShouldAddToEnrolledStudents()
    {
        // Arrange
        var course = Course.Create("Data Structures", "CS101", 3, 1, "Description", "Schedule", "Location");
        var student = new Student { Id = 1, StudentNumber = "2024CS001" };

        // Act
        course.EnrollStudent(student);

        // Assert
        Assert.Single(course.StudentCourses);
        var studentCourse = course.StudentCourses.First();
        Assert.Equal(student.Id, studentCourse.StudentId);
        Assert.Equal(course.Id, studentCourse.CourseId);
        Assert.True(studentCourse.IsActive);
    }

    [Fact]
    public void UnenrollStudent_ExistingStudent_ShouldRemoveFromEnrolledStudents()
    {
        // Arrange
        var course = Course.Create("Data Structures", "CS101", 3, 1, "Description", "Schedule", "Location");
        var student = new Student { Id = 1, StudentNumber = "2024CS001" };
        course.EnrollStudent(student);

        // Act
        course.UnenrollStudent(student);

        // Assert
        Assert.Empty(course.StudentCourses);
    }

    [Fact]
    public void GetEnrolledStudents_WithEnrolledStudents_ShouldReturnActiveStudents()
    {
        // Arrange
        var course = Course.Create("Data Structures", "CS101", 3, 1, "Description", "Schedule", "Location");
        var student1 = new Student { Id = 1, StudentNumber = "2024CS001" };
        var student2 = new Student { Id = 2, StudentNumber = "2024CS002" };
        
        course.EnrollStudent(student1);
        course.EnrollStudent(student2);

        // Act
        var enrolledStudents = course.GetEnrolledStudents();

        // Assert
        Assert.Equal(2, enrolledStudents.Count());
        Assert.Contains(student1, enrolledStudents);
        Assert.Contains(student2, enrolledStudents);
    }

    [Fact]
    public void GetEnrolledStudentCount_WithEnrolledStudents_ShouldReturnCorrectCount()
    {
        // Arrange
        var course = Course.Create("Data Structures", "CS101", 3, 1, "Description", "Schedule", "Location");
        var student1 = new Student { Id = 1, StudentNumber = "2024CS001" };
        var student2 = new Student { Id = 2, StudentNumber = "2024CS002" };
        
        course.EnrollStudent(student1);
        course.EnrollStudent(student2);

        // Act
        var count = course.GetEnrolledStudentCount();

        // Assert
        Assert.Equal(2, count);
    }
}
