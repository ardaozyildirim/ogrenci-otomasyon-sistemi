using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;
using StudentManagementSystem.Infrastructure.Data;

namespace StudentManagementSystem.Integration.Tests.Common;

public static class TestDataSeeder
{
    public static async Task<User> SeedUserAsync(ApplicationDbContext context, string email = "test@example.com", UserRole role = UserRole.Student)
    {
        var user = new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = email,
            Role = role,
            PasswordHash = "hashed_password",
            PhoneNumber = "1234567890",
            DateOfBirth = DateTime.Now.AddYears(-20),
            Address = "Test Address"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public static async Task<Student> SeedStudentAsync(ApplicationDbContext context, string studentNumber = "2024CS001")
    {
        var user = await SeedUserAsync(context, $"student{studentNumber}@example.com", UserRole.Student);

        var student = new Student
        {
            UserId = user.Id,
            StudentNumber = studentNumber,
            Department = "Computer Science",
            Grade = 10,
            ClassName = "A"
        };

        context.Students.Add(student);
        await context.SaveChangesAsync();
        return student;
    }

    public static async Task<Teacher> SeedTeacherAsync(ApplicationDbContext context, string employeeNumber = "EMP001")
    {
        var user = await SeedUserAsync(context, $"teacher{employeeNumber}@example.com", UserRole.Teacher);

        var teacher = new Teacher
        {
            UserId = user.Id,
            EmployeeNumber = employeeNumber,
            Department = "Computer Science",
            Specialization = "Software Engineering",
            HireDate = DateTime.Now
        };

        context.Teachers.Add(teacher);
        await context.SaveChangesAsync();
        return teacher;
    }

    public static async Task<Course> SeedCourseAsync(ApplicationDbContext context, string courseCode = "CS101")
    {
        var teacher = await SeedTeacherAsync(context);

        var course = new Course
        {
            Name = "Data Structures",
            Code = courseCode,
            Credits = 3,
            TeacherId = teacher.Id,
            Description = "Introduction to data structures",
            Schedule = "Mon, Wed, Fri 10:00-11:00",
            Location = "Room 101"
        };

        context.Courses.Add(course);
        await context.SaveChangesAsync();
        return course;
    }

    public static async Task<Grade> SeedGradeAsync(ApplicationDbContext context, int score = 85)
    {
        var student = await SeedStudentAsync(context);
        var course = await SeedCourseAsync(context);

        var grade = Grade.Create(student.Id, course.Id, score, "Test", "Test grade");

        context.Grades.Add(grade);
        await context.SaveChangesAsync();
        return grade;
    }

    public static async Task SeedCompleteTestDataAsync(ApplicationDbContext context)
    {
        // Seed multiple users
        await SeedUserAsync(context, "admin@example.com", UserRole.Admin);
        await SeedUserAsync(context, "student1@example.com", UserRole.Student);
        await SeedUserAsync(context, "student2@example.com", UserRole.Student);
        await SeedUserAsync(context, "teacher1@example.com", UserRole.Teacher);
        await SeedUserAsync(context, "teacher2@example.com", UserRole.Teacher);

        // Seed students
        await SeedStudentAsync(context, "2024CS001");
        await SeedStudentAsync(context, "2024CS002");
        await SeedStudentAsync(context, "2024MATH001");

        // Seed teachers
        await SeedTeacherAsync(context, "EMP001");
        await SeedTeacherAsync(context, "EMP002");

        // Seed courses
        await SeedCourseAsync(context, "CS101");
        await SeedCourseAsync(context, "CS102");
        await SeedCourseAsync(context, "MATH201");

        // Seed grades
        await SeedGradeAsync(context, 85);
        await SeedGradeAsync(context, 92);
        await SeedGradeAsync(context, 78);
    }
}
