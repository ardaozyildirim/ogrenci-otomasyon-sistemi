using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Domain.Tests.Entities;

public class TeacherTests
{
    [Fact]
    public void Create_ValidTeacher_ShouldSetProperties()
    {
        // Arrange
        var userId = 1;
        var employeeNumber = "EMP001";
        var department = "Computer Science";
        var specialization = "Software Engineering";
        var hireDate = DateTime.Now;

        // Act
        var teacher = Teacher.Create(userId, employeeNumber, department, specialization, hireDate);

        // Assert
        Assert.Equal(userId, teacher.UserId);
        Assert.Equal(employeeNumber, teacher.EmployeeNumber);
        Assert.Equal(department, teacher.Department);
        Assert.Equal(specialization, teacher.Specialization);
        Assert.Equal(hireDate, teacher.HireDate);
    }

    [Fact]
    public void AddCourse_ValidCourse_ShouldAddToCourses()
    {
        // Arrange
        var teacher = Teacher.Create(1, "EMP001", "Computer Science", "Software Engineering", DateTime.Now);
        var course = new Course
        {
            Id = 1,
            Name = "Data Structures",
            Code = "CS101",
            Credits = 3,
            TeacherId = teacher.Id
        };

        // Act
        teacher.AddCourse(course);

        // Assert
        Assert.Single(teacher.Courses);
        Assert.Contains(course, teacher.Courses);
    }

    [Fact]
    public void RemoveCourse_ExistingCourse_ShouldRemoveFromCourses()
    {
        // Arrange
        var teacher = Teacher.Create(1, "EMP001", "Computer Science", "Software Engineering", DateTime.Now);
        var course = new Course
        {
            Id = 1,
            Name = "Data Structures",
            Code = "CS101",
            Credits = 3,
            TeacherId = teacher.Id
        };
        teacher.AddCourse(course);

        // Act
        teacher.RemoveCourse(course);

        // Assert
        Assert.Empty(teacher.Courses);
    }

    [Fact]
    public void GetActiveCourses_WithActiveCourses_ShouldReturnActiveCourses()
    {
        // Arrange
        var teacher = Teacher.Create(1, "EMP001", "Computer Science", "Software Engineering", DateTime.Now);
        var activeCourse = new Course
        {
            Id = 1,
            Name = "Data Structures",
            Code = "CS101",
            Credits = 3,
            TeacherId = teacher.Id,
            Status = CourseStatus.Active
        };
        var completedCourse = new Course
        {
            Id = 2,
            Name = "Algorithms",
            Code = "CS102",
            Credits = 3,
            TeacherId = teacher.Id,
            Status = CourseStatus.Completed
        };

        teacher.AddCourse(activeCourse);
        teacher.AddCourse(completedCourse);

        // Act
        var activeCourses = teacher.GetActiveCourses();

        // Assert
        Assert.Single(activeCourses);
        Assert.Contains(activeCourse, activeCourses);
    }

    [Fact]
    public void GetCompletedCourses_WithCompletedCourses_ShouldReturnCompletedCourses()
    {
        // Arrange
        var teacher = Teacher.Create(1, "EMP001", "Computer Science", "Software Engineering", DateTime.Now);
        var activeCourse = new Course
        {
            Id = 1,
            Name = "Data Structures",
            Code = "CS101",
            Credits = 3,
            TeacherId = teacher.Id,
            Status = CourseStatus.Active
        };
        var completedCourse = new Course
        {
            Id = 2,
            Name = "Algorithms",
            Code = "CS102",
            Credits = 3,
            TeacherId = teacher.Id,
            Status = CourseStatus.Completed
        };

        teacher.AddCourse(activeCourse);
        teacher.AddCourse(completedCourse);

        // Act
        var completedCourses = teacher.GetCompletedCourses();

        // Assert
        Assert.Single(completedCourses);
        Assert.Contains(completedCourse, completedCourses);
    }
}
