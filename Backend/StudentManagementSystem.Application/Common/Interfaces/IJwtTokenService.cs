using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
    string? ValidateToken(string token);
}