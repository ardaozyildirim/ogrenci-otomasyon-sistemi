using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Infrastructure.Data;

namespace StudentManagementSystem.Infrastructure.Repositories;

public class AttendanceRepository : BaseRepository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Attendance>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Student)
            .ThenInclude(s => s.User)
            .Include(a => a.Course)
            .Where(a => a.StudentId == studentId && !a.IsDeleted)
            .OrderByDescending(a => a.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Attendance>> GetByCourseIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Student)
            .ThenInclude(s => s.User)
            .Include(a => a.Course)
            .Where(a => a.CourseId == courseId && !a.IsDeleted)
            .OrderByDescending(a => a.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Attendance>> GetByStudentAndCourseAsync(int studentId, int courseId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Student)
            .ThenInclude(s => s.User)
            .Include(a => a.Course)
            .Where(a => a.StudentId == studentId && a.CourseId == courseId && !a.IsDeleted)
            .OrderByDescending(a => a.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<Attendance?> GetByStudentCourseAndDateAsync(int studentId, int courseId, DateTime date, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Student)
            .ThenInclude(s => s.User)
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a => a.StudentId == studentId && a.CourseId == courseId && a.Date.Date == date.Date && !a.IsDeleted, cancellationToken);
    }
}