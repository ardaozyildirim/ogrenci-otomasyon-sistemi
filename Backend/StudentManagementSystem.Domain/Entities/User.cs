using StudentManagementSystem.Domain.Enums;
using StudentManagementSystem.Domain.ValueObjects;

namespace StudentManagementSystem.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public string? PhoneNumber { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public string? Address { get; private set; }
    
    public string FullName => $"{FirstName} {LastName}";

    public static User Create(string firstName, string lastName, string email, string passwordHash, UserRole role, 
        string? phoneNumber = null, DateTime? dateOfBirth = null, string? address = null)
    {
        ValidateUserCreationParams(firstName, lastName, passwordHash);

        return new User
        {
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHash,
            Role = role,
            PhoneNumber = phoneNumber?.Trim(),
            DateOfBirth = dateOfBirth,
            Address = address?.Trim()
        };
    }

    private static void ValidateUserCreationParams(string firstName, string lastName, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required", nameof(lastName));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required", nameof(passwordHash));
    }

    public void UpdateProfile(string firstName, string lastName, string? phoneNumber = null, 
        DateTime? dateOfBirth = null, string? address = null)
    {
        ValidateProfileUpdateParams(firstName, lastName);

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        PhoneNumber = phoneNumber?.Trim();
        DateOfBirth = dateOfBirth;
        Address = address?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateProfileUpdateParams(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required", nameof(lastName));
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Password hash is required", nameof(newPasswordHash));

        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool HasRole(UserRole role) => Role == role;
    public bool IsAdmin() => HasRole(UserRole.Admin);
    public bool IsTeacher() => HasRole(UserRole.Teacher);
    public bool IsStudent() => HasRole(UserRole.Student);
}
