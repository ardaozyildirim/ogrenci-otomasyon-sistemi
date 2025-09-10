using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Infrastructure.Data;

namespace StudentManagementSystem.Infrastructure.Repositories;

public class StudentRepository : BaseRepository<Student>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Student?> GetByStudentNumberAsync(string studentNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.StudentNumber == studentNumber && !s.IsDeleted, cancellationToken);
    }

    public async Task<Student?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.UserId == userId && !s.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Student>> GetByCourseIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.User)
            .Include(s => s.StudentCourses)
            .Where(s => s.StudentCourses.Any(sc => sc.CourseId == courseId && sc.IsActive) && !s.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Student>> GetByDepartmentAsync(string department, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.User)
            .Where(s => s.Department == department && !s.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> StudentNumberExistsAsync(string studentNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(s => s.StudentNumber == studentNumber && !s.IsDeleted, cancellationToken);
    }
}
