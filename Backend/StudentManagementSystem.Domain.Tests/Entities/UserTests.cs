using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Create_ValidUser_ShouldReturnUser()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var passwordHash = "hashedpassword";
        var role = UserRole.Student;

        // Act
        var user = User.Create(firstName, lastName, email, passwordHash, role);

        // Assert
        Assert.Equal(firstName, user.FirstName);
        Assert.Equal(lastName, user.LastName);
        Assert.Equal(email, user.Email);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.Equal(role, user.Role);
        Assert.Equal("John Doe", user.FullName);
    }

    [Theory]
    [InlineData("", "Doe", "john@example.com", "password", UserRole.Student)]
    [InlineData("John", "", "john@example.com", "password", UserRole.Student)]
    [InlineData("John", "Doe", "john@example.com", "", UserRole.Student)]
    public void Create_InvalidInput_ShouldThrowArgumentException(string firstName, string lastName, string email, string passwordHash, UserRole role)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => User.Create(firstName, lastName, email, passwordHash, role));
    }

    [Fact]
    public void UpdateProfile_ValidData_ShouldUpdateUser()
    {
        // Arrange
        var user = User.Create("John", "Doe", "john@example.com", "password", UserRole.Student);
        var newFirstName = "Jane";
        var newLastName = "Smith";
        var newPhoneNumber = "1234567890";

        // Act
        user.UpdateProfile(newFirstName, newLastName, newPhoneNumber);

        // Assert
        Assert.Equal(newFirstName, user.FirstName);
        Assert.Equal(newLastName, user.LastName);
        Assert.Equal(newPhoneNumber, user.PhoneNumber);
        Assert.NotNull(user.UpdatedAt);
    }

    [Fact]
    public void ChangePassword_ValidPassword_ShouldUpdatePassword()
    {
        // Arrange
        var user = User.Create("John", "Doe", "john@example.com", "oldpassword", UserRole.Student);
        var newPasswordHash = "newpasswordhash";

        // Act
        user.ChangePassword(newPasswordHash);

        // Assert
        Assert.Equal(newPasswordHash, user.PasswordHash);
        Assert.NotNull(user.UpdatedAt);
    }

    [Fact]
    public void IsAdmin_AdminUser_ShouldReturnTrue()
    {
        // Arrange
        var user = User.Create("Admin", "User", "admin@example.com", "password", UserRole.Admin);

        // Act & Assert
        Assert.True(user.IsAdmin());
        Assert.False(user.IsTeacher());
        Assert.False(user.IsStudent());
    }

    [Fact]
    public void IsTeacher_TeacherUser_ShouldReturnTrue()
    {
        // Arrange
        var user = User.Create("Teacher", "User", "teacher@example.com", "password", UserRole.Teacher);

        // Act & Assert
        Assert.False(user.IsAdmin());
        Assert.True(user.IsTeacher());
        Assert.False(user.IsStudent());
    }

    [Fact]
    public void IsStudent_StudentUser_ShouldReturnTrue()
    {
        // Arrange
        var user = User.Create("Student", "User", "student@example.com", "password", UserRole.Student);

        // Act & Assert
        Assert.False(user.IsAdmin());
        Assert.False(user.IsTeacher());
        Assert.True(user.IsStudent());
    }
}
