using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Enums;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface ICourseRepository : IRepository<Course>
{
    Task<Course?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<Course>> GetByTeacherIdAsync(int teacherId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Course>> GetByStatusAsync(CourseStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Course>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default);
    Task<bool> CourseCodeExistsAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<Course>> GetAllAsync(int? pageNumber = null, int? pageSize = null, int? teacherId = null, string? department = null, CancellationToken cancellationToken = default);
}
