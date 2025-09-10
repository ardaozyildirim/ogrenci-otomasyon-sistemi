using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IStudentRepository : IRepository<Student>
{
    Task<Student?> GetByStudentNumberAsync(string studentNumber, CancellationToken cancellationToken = default);
    Task<Student?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Student>> GetByCourseIdAsync(int courseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Student>> GetByDepartmentAsync(string department, CancellationToken cancellationToken = default);
    Task<bool> StudentNumberExistsAsync(string studentNumber, CancellationToken cancellationToken = default);
}
