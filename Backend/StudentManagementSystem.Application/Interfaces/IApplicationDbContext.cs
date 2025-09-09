using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Student> Students { get; }
    DbSet<Teacher> Teachers { get; }
    DbSet<Course> Courses { get; }
    DbSet<StudentCourse> StudentCourses { get; }
    DbSet<Grade> Grades { get; }
    DbSet<Attendance> Attendances { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
