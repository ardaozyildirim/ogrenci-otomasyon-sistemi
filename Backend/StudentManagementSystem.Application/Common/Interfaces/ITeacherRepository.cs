using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface ITeacherRepository
{
    Task<Teacher?> GetByIdAsync(int id);
    Task<IEnumerable<Teacher>> GetAllAsync();
    Task<Teacher?> GetByEmailAsync(string email);
    Task<Teacher?> GetByEmployeeIdAsync(string employeeId);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> EmployeeIdExistsAsync(string employeeId);
    Task<Teacher> AddAsync(Teacher teacher);
    Task<Teacher> UpdateAsync(Teacher teacher);
    Task DeleteAsync(Teacher teacher);
}