using StudentManagementSystem.Application.DTOs;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface IAttendanceRepository
{
    Task<Attendance?> GetByIdAsync(int id);
    Task<IEnumerable<Attendance>> GetAllAsync();
    Task<IEnumerable<Attendance>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<Attendance>> GetByCourseIdAsync(int courseId);
    Task<IEnumerable<Attendance>> GetByStudentAndCourseAsync(int studentId, int courseId);
    Task<IEnumerable<AttendanceSummaryDto>> GetAttendanceSummaryAsync(int? studentId = null, int? courseId = null);
    Task<Attendance> CreateAsync(Attendance attendance);
    Task<Attendance> UpdateAsync(Attendance attendance);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> StudentExistsInCourseAsync(int studentId, int courseId);
    Task<bool> AttendanceExistsForDateAsync(int studentId, int courseId, DateTime date);
}