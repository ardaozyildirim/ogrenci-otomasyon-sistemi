using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(int id);
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student?> GetByEmailAsync(string email);
    Task<Student?> GetByStudentNumberAsync(string studentNumber);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> StudentNumberExistsAsync(string studentNumber);
    Task<Student> AddAsync(Student student);
    Task<Student> UpdateAsync(Student student);
    Task DeleteAsync(Student student);
}