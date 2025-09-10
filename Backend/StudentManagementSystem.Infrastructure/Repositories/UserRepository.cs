using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Infrastructure.Data;

namespace StudentManagementSystem.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);
    }
}
