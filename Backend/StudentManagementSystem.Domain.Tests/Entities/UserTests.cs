using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void CreateUser_WithValidInformation_BuildsUserCorrectly()
    {
        // Set up user details
        var firstName = "Sarah";
        var lastName = "Johnson";
        var email = "sarah.johnson@university.edu";
        var password = "SecurePass2024";
        var role = UserRole.Teacher;

        // Create the user
        var newUser = User.Create(firstName, lastName, email, password, role);

        // Verify all properties
        Assert.Equal(firstName, newUser.FirstName);
        Assert.Equal(lastName, newUser.LastName);
        Assert.Equal(email, newUser.Email);
        Assert.Equal(password, newUser.PasswordHash);
        Assert.Equal(role, newUser.Role);
        Assert.Equal("Sarah Johnson", newUser.FullName);
    }

    [Fact]
    public void CreateUser_WithEmptyFirstName_ThrowsException()
    {
        // Try to create user with blank first name - this should fail
        Assert.Throws<ArgumentException>(() => 
            User.Create("", "Williams", "test@school.com", "mypassword", UserRole.Student));
    }

    [Fact]
    public void UpdateProfile_ChangesNameInformation()
    {
        // Start with existing user
        var user = User.Create("Michael", "Brown", "m.brown@college.edu", "password123", UserRole.Student);
        var updatedFirstName = "Mike";
        var updatedLastName = "Brown-Smith";

        // Update their profile
        user.UpdateProfile(updatedFirstName, updatedLastName);

        // Check the changes took effect
        Assert.Equal(updatedFirstName, user.FirstName);
        Assert.Equal(updatedLastName, user.LastName);
        Assert.NotNull(user.UpdatedAt);
    }

    [Fact]
    public void ChangePassword_UpdatesUserPassword()
    {
        // Create user with initial password
        var user = User.Create("Emma", "Davis", "emma.davis@uni.edu", "oldPassword123", UserRole.Student);
        var betterPassword = "NewSecurePassword2024!";

        // Change to new password
        user.ChangePassword(betterPassword);

        // Verify password was updated
        Assert.Equal(betterPassword, user.PasswordHash);
    }

    [Fact]
    public void IsAdmin_ForAdminUser_ReturnsTrue()
    {
        // Create an admin user
        var adminUser = User.Create("Robert", "Chen", "r.chen@admin.edu", "adminpass", UserRole.Admin);

        // Test role checking methods
        Assert.True(adminUser.IsAdmin());
        Assert.False(adminUser.IsTeacher());
        Assert.False(adminUser.IsStudent());
    }

    [Fact]
    public void IsStudent_ForStudentUser_ReturnsTrue()
    {
        // Set up a student account
        var studentUser = User.Create("Lisa", "Rodriguez", "lisa.r@student.edu", "studentpass", UserRole.Student);

        // Verify role identification works correctly
        Assert.True(studentUser.IsStudent());
        Assert.False(studentUser.IsAdmin());
        Assert.False(studentUser.IsTeacher());
    }

    [Fact] 
    public void UserFullName_CombinesFirstAndLastName()
    {
        // Create user with specific names
        var user = User.Create("Alexander", "Smith-Johnson", "alex@college.edu", "secure123", UserRole.Teacher);
        
        // Full name should combine both parts
        Assert.Equal("Alexander Smith-Johnson", user.FullName);
    }
}
