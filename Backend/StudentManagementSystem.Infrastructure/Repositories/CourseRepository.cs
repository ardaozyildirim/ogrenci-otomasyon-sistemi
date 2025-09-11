using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Infrastructure.Data;

namespace StudentManagementSystem.Infrastructure.Repositories;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Course?> GetByCourseCodeAsync(string courseCode)
    {
        return await _context.Courses
            .Include(c => c.Teacher)
            .FirstOrDefaultAsync(c => c.CourseCode == courseCode);
    }

    public async Task<IEnumerable<Course>> GetByTeacherIdAsync(int teacherId)
    {
        return await _context.Courses
            .Include(c => c.Teacher)
            .Where(c => c.TeacherId == teacherId)
            .ToListAsync();
    }

    public async Task<bool> CourseCodeExistsAsync(string courseCode)
    {
        return await _context.Courses
            .AnyAsync(c => c.CourseCode == courseCode);
    }

    public override async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Courses
            .Include(c => c.Teacher)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public override async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses
            .Include(c => c.Teacher)
            .ToListAsync();
    }

    public async Task<IEnumerable<Student>> GetCourseStudentsAsync(int courseId)
    {
        return await _context.StudentCourses
            .Where(sc => sc.CourseId == courseId)
            .Include(sc => sc.Student)
            .Select(sc => sc.Student)
            .ToListAsync();
    }

    public async Task<bool> IsStudentEnrolledAsync(int courseId, int studentId)
    {
        return await _context.StudentCourses
            .AnyAsync(sc => sc.CourseId == courseId && sc.StudentId == studentId);
    }

    public async Task EnrollStudentAsync(int courseId, int studentId)
    {
        // Check if already enrolled
        if (await IsStudentEnrolledAsync(courseId, studentId))
        {
            throw new InvalidOperationException("Student is already enrolled in this course.");
        }

        // Check course capacity
        var enrolledCount = await GetEnrolledStudentsCountAsync(courseId);
        var course = await GetByIdAsync(courseId);
        
        if (course != null && enrolledCount >= course.Capacity)
        {
            throw new InvalidOperationException("Course has reached maximum capacity.");
        }

        var studentCourse = new StudentCourse
        {
            CourseId = courseId,
            StudentId = studentId,
            EnrollmentDate = DateTime.UtcNow
        };

        await _context.StudentCourses.AddAsync(studentCourse);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveStudentAsync(int courseId, int studentId)
    {
        var studentCourse = await _context.StudentCourses
            .FirstOrDefaultAsync(sc => sc.CourseId == courseId && sc.StudentId == studentId);

        if (studentCourse == null)
        {
            throw new InvalidOperationException("Student is not enrolled in this course.");
        }

        _context.StudentCourses.Remove(studentCourse);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetEnrolledStudentsCountAsync(int courseId)
    {
        return await _context.StudentCourses
            .CountAsync(sc => sc.CourseId == courseId);
    }
}