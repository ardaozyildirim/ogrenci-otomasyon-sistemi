using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Infrastructure.Data;
using StudentManagementSystem.Infrastructure.Repositories.Interfaces;

namespace StudentManagementSystem.Infrastructure.Repositories;

public class StudentRepository : Repository<Student>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Student?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.Email.ToLower() == email.ToLower());
    }

    public async Task<Student?> GetByStudentNumberAsync(string studentNumber)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbSet.AnyAsync(s => s.Email.ToLower() == email.ToLower());
    }

    public async Task<bool> StudentNumberExistsAsync(string studentNumber)
    {
        return await _dbSet.AnyAsync(s => s.StudentNumber == studentNumber);
    }
}