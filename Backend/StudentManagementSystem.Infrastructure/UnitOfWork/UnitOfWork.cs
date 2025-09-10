using Microsoft.EntityFrameworkCore.Storage;
using StudentManagementSystem.Application.Common.Interfaces;
using StudentManagementSystem.Infrastructure.Data;
using StudentManagementSystem.Infrastructure.Repositories;

namespace StudentManagementSystem.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        Students = new StudentRepository(_context);
        Teachers = new TeacherRepository(_context);
        Courses = new CourseRepository(_context);
        Grades = new GradeRepository(_context);
    }

    public IUserRepository Users { get; }
    public IStudentRepository Students { get; }
    public ITeacherRepository Teachers { get; }
    public ICourseRepository Courses { get; }
    public IGradeRepository Grades { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
