using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
    string GenerateToken(string email, string role, int userId);
}
