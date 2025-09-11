using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Domain.Tests.Entities;

public class TeacherTests
{
    [Fact]
    public void CreateTeacher_WithValidData_BuildsTeacherCorrectly()
    {
        // Set up teacher information
        int userId = 23;
        string employeeNumber = "EMP0256";
        string department = "Mathematics";
        string specialization = "Applied Statistics";
        DateTime hireDate = new DateTime(2018, 9, 15);

        // Create the teacher
        var teacher = Teacher.Create(userId, employeeNumber, department, specialization, hireDate);

        // Verify all the details
        Assert.Equal(userId, teacher.UserId);
        Assert.Equal(employeeNumber, teacher.EmployeeNumber);
        Assert.Equal(department, teacher.Department);
        Assert.Equal(specialization, teacher.Specialization);
        Assert.Equal(hireDate, teacher.HireDate);
    }

    [Fact]
    public void AddCourse_AssignsCourseToTeacher()
    {
        // Create a chemistry teacher
        var teacher = Teacher.Create(11, "EMP0087", "Chemistry", "Organic Chemistry", DateTime.Now.AddYears(-3));
        var course = Course.Create("General Chemistry", "CHEM101", 4, teacher.Id);

        // Assign course to teacher
        teacher.AddCourse(course);

        // Teacher should now have this course
        Assert.Single(teacher.Courses);
        Assert.Contains(course, teacher.Courses);
    }

    [Fact]
    public void RemoveCourse_UnassignsCourseFromTeacher()
    {
        // Set up teacher with a course
        var teacher = Teacher.Create(33, "EMP0156", "Physics", "Quantum Mechanics", DateTime.Now.AddYears(-5));
        var course = Course.Create("Intro to Physics", "PHYS100", 3, teacher.Id);
        teacher.AddCourse(course);

        // Remove the course
        teacher.RemoveCourse(course.Id);

        // Teacher should have no courses now
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
