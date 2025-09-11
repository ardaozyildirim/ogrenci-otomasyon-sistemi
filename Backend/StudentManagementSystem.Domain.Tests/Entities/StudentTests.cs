using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Events.Student;

namespace StudentManagementSystem.Domain.Tests.Entities;

public class StudentTests
{
    [Fact]
    public void Create_ValidStudent_ShouldRaiseStudentCreatedEvent()
    {
        // Arrange
        var userId = 1;
        var studentNumber = "2024CS001";
        var department = "Computer Science";

        // Act
        var student = Student.Create(userId, studentNumber, department);

        // Assert
        Assert.Equal(studentNumber, student.StudentNumber);
        Assert.Equal(department, student.Department);
        Assert.Single(student.DomainEvents);
        Assert.IsType<StudentCreatedEvent>(student.DomainEvents.First());
    }

    [Fact]
    public void EnrollInCourse_ValidCourse_ShouldRaiseStudentEnrolledInCourseEvent()
    {
        // Arrange
        var student = Student.Create(1, "2024CS001", "Computer Science");
        var courseId = 1;
        var courseName = "Data Structures";

        // Act
        student.EnrollInCourse(courseId, courseName);

        // Assert
        Assert.Equal(2, student.DomainEvents.Count);
        Assert.IsType<StudentEnrolledInCourseEvent>(student.DomainEvents.Last());
        
        var enrollmentEvent = student.DomainEvents.Last() as StudentEnrolledInCourseEvent;
        Assert.Equal(courseId, enrollmentEvent!.CourseId);
        Assert.Equal(courseName, enrollmentEvent.CourseName);
    }

    [Fact]
    public void CalculateGPA_WithGrades_ShouldReturnCorrectGPA()
    {
        // Arrange
        var student = Student.Create(1, "2024CS001", "Computer Science");
        
        // Mock grades
        student.Grades.Add(new Grade { Score = 80, Course = new Course { Credits = 3 } });
        student.Grades.Add(new Grade { Score = 90, Course = new Course { Credits = 3 } });
        
        // Mock student courses
        student.StudentCourses.Add(new StudentCourse 
        { 
            IsActive = true, 
            Course = new Course { Credits = 3 } 
        });
        student.StudentCourses.Add(new StudentCourse 
        { 
            IsActive = true, 
            Course = new Course { Credits = 3 } 
        });

        // Act
        var gpa = student.CalculateGPA();

        // Assert
        Assert.Equal(3.5m, gpa); // (3.0 + 4.0) / 2 = 3.5
    }

    [Fact]
    public void CalculateGPA_NoGrades_ShouldReturnZero()
    {
        // Arrange
        var student = Student.Create(1, "2024CS001", "Computer Science");

        // Act
        var gpa = student.CalculateGPA();

        // Assert
        Assert.Equal(0, gpa);
    }
}
