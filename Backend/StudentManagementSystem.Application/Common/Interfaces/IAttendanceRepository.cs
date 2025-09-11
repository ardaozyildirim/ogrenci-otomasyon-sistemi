using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IAttendanceRepository : IRepository<Attendance>
{
    Task<IEnumerable<Attendance>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Attendance>> GetByCourseIdAsync(int courseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Attendance>> GetByStudentAndCourseAsync(int studentId, int courseId, CancellationToken cancellationToken = default);
    Task<Attendance?> GetByStudentCourseAndDateAsync(int studentId, int courseId, DateTime date, CancellationToken cancellationToken = default);
}