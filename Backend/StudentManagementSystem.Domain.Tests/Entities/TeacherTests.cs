using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Domain.Tests.Entities;

public class TeacherTests
{
    [Fact]
    public void Create_ValidTeacher_ShouldWork()
    {
        // Arrange
        int userId = 1;
        string employeeNumber = "EMP0001";
        string department = "Computer Science";
        string specialization = "Software Engineering";
        DateTime hireDate = DateTime.Now;

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
    public void AddCourse_ShouldAddCourseToTeacher()
    {
        // Arrange
        var teacher = Teacher.Create(1, "EMP0001", "Computer Science", "Software Engineering", DateTime.Now);
        var course = Course.Create("Math 101", "MATH101", 3, teacher.Id);

        // Act
        teacher.AddCourse(course);

        // Assert
        Assert.Single(teacher.Courses);
        Assert.Contains(course, teacher.Courses);
    }

    [Fact]
    public void RemoveCourse_ShouldRemoveCourseFromTeacher()
    {
        // Arrange
        var teacher = Teacher.Create(1, "EMP0001", "Computer Science", "Software Engineering", DateTime.Now);
        var course = Course.Create("Math 101", "MATH101", 3, teacher.Id);
        teacher.AddCourse(course);

        // Act
        teacher.RemoveCourse(course.Id);

        // Assert
        Assert.Empty(teacher.Courses);
    }

    [Fact]
    public void GetActiveCourses_ShouldReturnOnlyActiveCourses()
    {
        // Arrange
        var teacher = Teacher.Create(1, "EMP0001", "Computer Science", "Software Engineering", DateTime.Now);
        var activeCourse = Course.Create("Math 101", "MATH101", 3, teacher.Id);
        var completedCourse = Course.Create("Physics 101", "PHYS101", 3, teacher.Id);
        
        activeCourse.StartCourse();
        completedCourse.StartCourse();
        completedCourse.CompleteCourse();
        
        teacher.AddCourse(activeCourse);
        teacher.AddCourse(completedCourse);

        // Act
        var activeCourses = teacher.GetActiveCourses();

        // Assert
        Assert.Single(activeCourses);
        Assert.Contains(activeCourse, activeCourses);
    }

    [Fact]
    public void GetCompletedCourses_ShouldReturnOnlyCompletedCourses()
    {
        // Arrange
        var teacher = Teacher.Create(1, "EMP0001", "Computer Science", "Software Engineering", DateTime.Now);
        var activeCourse = Course.Create("Math 101", "MATH101", 3, teacher.Id);
        var completedCourse = Course.Create("Physics 101", "PHYS101", 3, teacher.Id);
        
        activeCourse.StartCourse();
        completedCourse.StartCourse();
        completedCourse.CompleteCourse();
        
        teacher.AddCourse(activeCourse);
        teacher.AddCourse(completedCourse);

        // Act
        var completedCourses = teacher.GetCompletedCourses();

        // Assert
        Assert.Single(completedCourses);
        Assert.Contains(completedCourse, completedCourses);
    }
}
