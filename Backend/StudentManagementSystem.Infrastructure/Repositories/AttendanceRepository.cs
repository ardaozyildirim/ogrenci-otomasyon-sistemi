using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Infrastructure.Data;

namespace StudentManagementSystem.Infrastructure.Repositories;

public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Attendance?> GetByIdAsync(int id)
    {
        return await _context.Attendances
            .Include(a => a.Student)
            .Include(a => a.Course)
                .ThenInclude(c => c.Teacher)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Attendance>> GetAllAsync()
    {
        return await _context.Attendances
            .Include(a => a.Student)
            .Include(a => a.Course)
                .ThenInclude(c => c.Teacher)
            .OrderByDescending(a => a.AttendanceDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attendance>> GetByStudentIdAsync(int studentId)
    {
        return await _context.Attendances
            .Include(a => a.Course)
                .ThenInclude(c => c.Teacher)
            .Where(a => a.StudentId == studentId)
            .OrderByDescending(a => a.AttendanceDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attendance>> GetByCourseIdAsync(int courseId)
    {
        return await _context.Attendances
            .Include(a => a.Student)
            .Include(a => a.Course)
            .Where(a => a.CourseId == courseId)
            .OrderBy(a => a.Student.LastName)
                .ThenBy(a => a.Student.FirstName)
                .ThenByDescending(a => a.AttendanceDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attendance>> GetByStudentAndCourseAsync(int studentId, int courseId)
    {
        return await _context.Attendances
            .Include(a => a.Course)
                .ThenInclude(c => c.Teacher)
            .Where(a => a.StudentId == studentId && a.CourseId == courseId)
            .OrderByDescending(a => a.AttendanceDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<AttendanceSummaryDto>> GetAttendanceSummaryAsync(int? studentId = null, int? courseId = null)
    {
        var query = _context.Attendances
            .Include(a => a.Student)
            .Include(a => a.Course)
            .AsQueryable();

        if (studentId.HasValue)
        {
            query = query.Where(a => a.StudentId == studentId.Value);
        }

        if (courseId.HasValue)
        {
            query = query.Where(a => a.CourseId == courseId.Value);
        }

        var groupedData = await query
            .GroupBy(a => new { a.StudentId, a.CourseId })
            .Select(g => new AttendanceSummaryDto
            {
                StudentId = g.Key.StudentId,
                StudentName = g.First().Student.FirstName + " " + g.First().Student.LastName,
                StudentNumber = g.First().Student.StudentNumber,
                CourseId = g.Key.CourseId,
                CourseName = g.First().Course.Name,
                TotalSessions = g.Count(),
                PresentSessions = g.Count(a => a.IsPresent),
                AbsentSessions = g.Count(a => !a.IsPresent),
                AttendancePercentage = Math.Round((decimal)g.Count(a => a.IsPresent) / g.Count() * 100, 2)
            })
            .ToListAsync();

        return groupedData;
    }

    public async Task<Attendance> CreateAsync(Attendance attendance)
    {
        attendance.CreatedAt = DateTime.UtcNow;
        if (attendance.AttendanceDate == default)
        {
            attendance.AttendanceDate = DateTime.UtcNow;
        }

        await _context.Attendances.AddAsync(attendance);
        await _context.SaveChangesAsync();
        
        return await GetByIdAsync(attendance.Id) ?? attendance;
    }

    public async Task<Attendance> UpdateAsync(Attendance attendance)
    {
        attendance.UpdatedAt = DateTime.UtcNow;
        _context.Attendances.Update(attendance);
        await _context.SaveChangesAsync();
        
        return await GetByIdAsync(attendance.Id) ?? attendance;
    }

    public async Task DeleteAsync(int id)
    {
        var attendance = await _context.Attendances.FindAsync(id);
        if (attendance != null)
        {
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Attendances.AnyAsync(a => a.Id == id);
    }

    public async Task<bool> StudentExistsInCourseAsync(int studentId, int courseId)
    {
        return await _context.StudentCourses
            .AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId && sc.IsActive);
    }

    public async Task<bool> AttendanceExistsForDateAsync(int studentId, int courseId, DateTime date)
    {
        return await _context.Attendances
            .AnyAsync(a => a.StudentId == studentId && a.CourseId == courseId && a.AttendanceDate.Date == date.Date);
    }
}