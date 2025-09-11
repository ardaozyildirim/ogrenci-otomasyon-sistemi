using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IGradeRepository
{
    Task<Grade?> GetByIdAsync(int id);
    Task<IEnumerable<Grade>> GetAllAsync();
    Task<IEnumerable<Grade>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<Grade>> GetByCourseIdAsync(int courseId);
    Task<IEnumerable<Grade>> GetByStudentAndCourseAsync(int studentId, int courseId);
    Task<Grade> CreateAsync(Grade grade);
    Task<Grade> UpdateAsync(Grade grade);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> StudentExistsInCourseAsync(int studentId, int courseId);

    // Soft delete specific methods
    Task<Grade?> GetByIdIncludingDeletedAsync(int id);
    Task<IEnumerable<Grade>> GetAllIncludingDeletedAsync();
    Task<IEnumerable<Grade>> GetDeletedAsync();
    Task RestoreAsync(int id);
    Task HardDeleteAsync(int id);
}