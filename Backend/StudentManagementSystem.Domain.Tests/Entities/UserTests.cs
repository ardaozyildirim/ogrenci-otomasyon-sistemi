using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Create_ValidUser_ShouldWork()
    {
        // Arrange
        string firstName = "John";
        string lastName = "Doe";
        string email = "john@test.com";
        string password = "password123";
        UserRole role = UserRole.Student;

        // Act
        var user = User.Create(firstName, lastName, email, password, role);

        // Assert
        Assert.Equal(firstName, user.FirstName);
        Assert.Equal(lastName, user.LastName);
        Assert.Equal(email, user.Email);
        Assert.Equal(password, user.PasswordHash);
        Assert.Equal(role, user.Role);
        Assert.Equal("John Doe", user.FullName);
    }

    [Fact]
    public void Create_EmptyFirstName_ShouldThrowError()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            User.Create("", "Doe", "john@test.com", "password", UserRole.Student));
    }

    [Fact]
    public void UpdateProfile_ShouldChangeUserInfo()
    {
        // Arrange
        var user = User.Create("John", "Doe", "john@test.com", "password", UserRole.Student);
        string newFirstName = "Jane";
        string newLastName = "Smith";

        // Act
        user.UpdateProfile(newFirstName, newLastName);

        // Assert
        Assert.Equal(newFirstName, user.FirstName);
        Assert.Equal(newLastName, user.LastName);
        Assert.NotNull(user.UpdatedAt);
    }

    [Fact]
    public void ChangePassword_ShouldUpdatePassword()
    {
        // Arrange
        var user = User.Create("John", "Doe", "john@test.com", "oldpassword", UserRole.Student);
        string newPassword = "newpassword";

        // Act
        user.ChangePassword(newPassword);

        // Assert
        Assert.Equal(newPassword, user.PasswordHash);
    }

    [Fact]
    public void IsAdmin_AdminUser_ShouldReturnTrue()
    {
        // Arrange
        var user = User.Create("Admin", "User", "admin@test.com", "password", UserRole.Admin);

        // Act & Assert
        Assert.True(user.IsAdmin());
        Assert.False(user.IsTeacher());
        Assert.False(user.IsStudent());
    }

    [Fact]
    public void IsStudent_StudentUser_ShouldReturnTrue()
    {
        // Arrange
        var user = User.Create("Student", "User", "student@test.com", "password", UserRole.Student);

        // Act & Assert
        Assert.True(user.IsStudent());
        Assert.False(user.IsAdmin());
        Assert.False(user.IsTeacher());
    }
}
