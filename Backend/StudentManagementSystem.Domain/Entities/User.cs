using StudentManagementSystem.Domain.Enums;
using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";

    public static User Create(string firstName, string lastName, string email, string passwordHash, UserRole role, string? phoneNumber = null, DateTime? dateOfBirth = null, string? address = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be null or empty", nameof(passwordHash));

        return new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = Email.Create(email).Value,
            PasswordHash = passwordHash,
            Role = role,
            PhoneNumber = phoneNumber,
            DateOfBirth = dateOfBirth,
            Address = address
        };
    }

    public void UpdateProfile(string firstName, string lastName, string? phoneNumber = null, DateTime? dateOfBirth = null, string? address = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Password hash cannot be null or empty", nameof(newPasswordHash));

        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsAdmin() => Role == UserRole.Admin;
    public bool IsTeacher() => Role == UserRole.Teacher;
    public bool IsStudent() => Role == UserRole.Student;
}
