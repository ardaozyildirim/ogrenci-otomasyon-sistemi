using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Common.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(int id);
    Task<IEnumerable<Course>> GetAllAsync();
    Task<Course?> GetByCourseCodeAsync(string courseCode);
    Task<IEnumerable<Course>> GetByTeacherIdAsync(int teacherId);
    Task<bool> CourseCodeExistsAsync(string courseCode);
    Task<Course> AddAsync(Course course);
    Task<Course> UpdateAsync(Course course);
    Task DeleteAsync(Course course);
    Task<IEnumerable<Student>> GetCourseStudentsAsync(int courseId);
    Task<bool> IsStudentEnrolledAsync(int courseId, int studentId);
    Task EnrollStudentAsync(int courseId, int studentId);
    Task RemoveStudentAsync(int courseId, int studentId);
    Task<int> GetEnrolledStudentsCountAsync(int courseId);
}