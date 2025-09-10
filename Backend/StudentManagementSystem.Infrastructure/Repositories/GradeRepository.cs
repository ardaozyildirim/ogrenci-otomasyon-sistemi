using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Infrastructure.Data;

namespace StudentManagementSystem.Infrastructure.Repositories;

public class GradeRepository : BaseRepository<Grade>, IGradeRepository
{
    public GradeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Grade>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(g => g.Student)
            .ThenInclude(s => s.User)
            .Include(g => g.Course)
            .Where(g => g.StudentId == studentId && !g.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Grade>> GetByCourseIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(g => g.Student)
            .ThenInclude(s => s.User)
            .Include(g => g.Course)
            .Where(g => g.CourseId == courseId && !g.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<Grade?> GetByStudentAndCourseAsync(int studentId, int courseId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(g => g.Student)
            .ThenInclude(s => s.User)
            .Include(g => g.Course)
            .FirstOrDefaultAsync(g => g.StudentId == studentId && g.CourseId == courseId && !g.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Grade>> GetByStudentAndCourseIdAsync(int studentId, int courseId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(g => g.Student)
            .ThenInclude(s => s.User)
            .Include(g => g.Course)
            .Where(g => g.StudentId == studentId && g.CourseId == courseId && !g.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}
