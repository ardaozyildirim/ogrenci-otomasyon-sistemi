using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IGradeRepository : IRepository<Grade>
{
    Task<IEnumerable<Grade>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Grade>> GetByCourseIdAsync(int courseId, CancellationToken cancellationToken = default);
    Task<Grade?> GetByStudentAndCourseAsync(int studentId, int courseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Grade>> GetByStudentAndCourseIdAsync(int studentId, int courseId, CancellationToken cancellationToken = default);
}
