using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Infrastructure.Data;

namespace StudentManagementSystem.Infrastructure.Repositories;

public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
{
    public TeacherRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Teacher?> GetByEmployeeNumberAsync(string employeeNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.EmployeeNumber == employeeNumber && !t.IsDeleted, cancellationToken);
    }

    public async Task<Teacher?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.UserId == userId && !t.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Teacher>> GetByDepartmentAsync(string department, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.User)
            .Where(t => t.Department == department && !t.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> EmployeeNumberExistsAsync(string employeeNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(t => t.EmployeeNumber == employeeNumber && !t.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Teacher>> GetAllAsync(int? pageNumber = null, int? pageSize = null, string? department = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(t => t.User)
            .Where(t => !t.IsDeleted);

        if (!string.IsNullOrEmpty(department))
            query = query.Where(t => t.Department == department);

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            var skip = (pageNumber.Value - 1) * pageSize.Value;
            query = query.Skip(skip).Take(pageSize.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }
}
