using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;
using StudentManagementSystem.Infrastructure.Data;

namespace StudentManagementSystem.Infrastructure.Repositories;

public class CourseRepository : BaseRepository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Course?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Teacher)
            .ThenInclude(t => t.User)
            .FirstOrDefaultAsync(c => c.Code == code && !c.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetByTeacherIdAsync(int teacherId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Teacher)
            .ThenInclude(t => t.User)
            .Where(c => c.TeacherId == teacherId && !c.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetByStatusAsync(CourseStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Teacher)
            .ThenInclude(t => t.User)
            .Where(c => c.Status == status && !c.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Teacher)
            .ThenInclude(t => t.User)
            .Include(c => c.StudentCourses)
            .Where(c => c.StudentCourses.Any(sc => sc.StudentId == studentId && sc.IsActive) && !c.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> CourseCodeExistsAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(c => c.Code == code && !c.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetAllAsync(int? pageNumber = null, int? pageSize = null, int? teacherId = null, string? department = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(c => c.Teacher)
            .ThenInclude(t => t.User)
            .Where(c => !c.IsDeleted);

        if (teacherId.HasValue)
            query = query.Where(c => c.TeacherId == teacherId);

        if (!string.IsNullOrEmpty(department))
            query = query.Where(c => c.Teacher.Department == department);

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            var skip = (pageNumber.Value - 1) * pageSize.Value;
            query = query.Skip(skip).Take(pageSize.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }
}
