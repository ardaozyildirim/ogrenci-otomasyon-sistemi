using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Infrastructure.Data;

namespace StudentManagementSystem.Infrastructure.Repositories;

public class TeacherRepository : Repository<Teacher>, ITeacherRepository
{
    public TeacherRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Teacher?> GetByEmailAsync(string email)
    {
        return await _context.Teachers
            .FirstOrDefaultAsync(t => t.Email == email);
    }

    public async Task<Teacher?> GetByEmployeeIdAsync(string employeeId)
    {
        return await _context.Teachers
            .FirstOrDefaultAsync(t => t.EmployeeId == employeeId);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Teachers
            .AnyAsync(t => t.Email == email);
    }

    public async Task<bool> EmployeeIdExistsAsync(string employeeId)
    {
        return await _context.Teachers
            .AnyAsync(t => t.EmployeeId == employeeId);
    }
}