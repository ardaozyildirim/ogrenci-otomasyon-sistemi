using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Infrastructure.Data;

namespace StudentManagementSystem.Infrastructure.Repositories;

public class GradeRepository : Repository<Grade>, IGradeRepository
{
    public GradeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Grade?> GetByIdAsync(int id)
    {
        return await _context.Grades
            .Include(g => g.Student)
            .Include(g => g.Course)
                .ThenInclude(c => c.Teacher)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Grade>> GetAllAsync()
    {
        return await _context.Grades
            .Include(g => g.Student)
            .Include(g => g.Course)
                .ThenInclude(c => c.Teacher)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetByStudentIdAsync(int studentId)
    {
        return await _context.Grades
            .Include(g => g.Course)
                .ThenInclude(c => c.Teacher)
            .Where(g => g.StudentId == studentId)
            .OrderByDescending(g => g.GradeDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetByCourseIdAsync(int courseId)
    {
        return await _context.Grades
            .Include(g => g.Student)
            .Include(g => g.Course)
            .Where(g => g.CourseId == courseId)
            .OrderBy(g => g.Student.LastName)
                .ThenBy(g => g.Student.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetByStudentAndCourseAsync(int studentId, int courseId)
    {
        return await _context.Grades
            .Include(g => g.Course)
                .ThenInclude(c => c.Teacher)
            .Where(g => g.StudentId == studentId && g.CourseId == courseId)
            .OrderByDescending(g => g.GradeDate)
            .ToListAsync();
    }

    public async Task<Grade> CreateAsync(Grade grade)
    {
        grade.CreatedAt = DateTime.UtcNow;
        if (grade.GradeDate == default)
        {
            grade.GradeDate = DateTime.UtcNow;
        }

        await _context.Grades.AddAsync(grade);
        await _context.SaveChangesAsync();
        
        return await GetByIdAsync(grade.Id) ?? grade;
    }

    public async Task<Grade> UpdateAsync(Grade grade)
    {
        grade.UpdatedAt = DateTime.UtcNow;
        _context.Grades.Update(grade);
        await _context.SaveChangesAsync();
        
        return await GetByIdAsync(grade.Id) ?? grade;
    }

    public async Task DeleteAsync(int id)
    {
        var grade = await _context.Grades.FindAsync(id);
        if (grade != null)
        {
            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Grades.AnyAsync(g => g.Id == id);
    }

    public async Task<bool> StudentExistsInCourseAsync(int studentId, int courseId)
    {
        return await _context.StudentCourses
            .AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId && sc.IsActive);
    }

    // Soft delete specific methods
    public async Task<Grade?> GetByIdIncludingDeletedAsync(int id)
    {
        return await _context.Grades
            .IgnoreQueryFilters()
            .Include(g => g.Student)
            .Include(g => g.Course)
                .ThenInclude(c => c.Teacher)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Grade>> GetAllIncludingDeletedAsync()
    {
        return await _context.Grades
            .IgnoreQueryFilters()
            .Include(g => g.Student)
            .Include(g => g.Course)
                .ThenInclude(c => c.Teacher)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Grade>> GetDeletedAsync()
    {
        return await _context.Grades
            .IgnoreQueryFilters()
            .Include(g => g.Student)
            .Include(g => g.Course)
                .ThenInclude(c => c.Teacher)
            .Where(g => g.IsDeleted)
            .OrderByDescending(g => g.DeletedAt)
            .ToListAsync();
    }

    public async Task RestoreAsync(int id)
    {
        var grade = await _context.Grades
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(g => g.Id == id && g.IsDeleted);
        
        if (grade != null)
        {
            grade.IsDeleted = false;
            grade.DeletedAt = null;
            grade.DeletedBy = null;
            grade.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
        }
    }

    public async Task HardDeleteAsync(int id)
    {
        var grade = await _context.Grades
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(g => g.Id == id);
        
        if (grade != null)
        {
            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
        }
    }
}