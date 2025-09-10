using StudentManagementSystem.Application.Common.Interfaces;

namespace StudentManagementSystem.Infrastructure.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IStudentRepository Students { get; }
    ITeacherRepository Teachers { get; }
    ICourseRepository Courses { get; }
    IGradeRepository Grades { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
