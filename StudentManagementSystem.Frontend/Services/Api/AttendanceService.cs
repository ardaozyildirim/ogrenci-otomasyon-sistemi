using StudentManagementSystem.Frontend.Models.DTOs;

namespace StudentManagementSystem.Frontend.Services.Api;

public class AttendanceService
{
    private readonly ApiService _apiService;

    public AttendanceService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<AttendanceDto>?> GetStudentAttendanceAsync(int studentId, int? courseId = null)
    {
        var endpoint = $"api/v1.0/attendance/student/{studentId}";
        if (courseId.HasValue)
        {
            endpoint += $"?courseId={courseId}";
        }
        
        var response = await _apiService.GetWithResponseAsync<List<AttendanceDto>>(endpoint);
        return response?.Data;
    }

    public async Task<List<AttendanceDto>?> GetCourseAttendanceAsync(int courseId, DateTime? date = null)
    {
        var endpoint = $"api/v1.0/attendance/course/{courseId}";
        if (date.HasValue)
        {
            endpoint += $"?date={date.Value:yyyy-MM-dd}";
        }
        
        var response = await _apiService.GetWithResponseAsync<List<AttendanceDto>>(endpoint);
        return response?.Data;
    }

    public async Task<AttendanceDto?> RecordAttendanceAsync(RecordAttendanceRequest request)
    {
        var response = await _apiService.PostWithResponseAsync<AttendanceDto>("api/v1.0/attendance", request);
        return response?.Data;
    }

    public async Task<bool> UpdateAttendanceAsync(int id, UpdateAttendanceRequest request)
    {
        request.Id = id;
        var response = await _apiService.PutAsync<AttendanceDto>($"api/v1.0/attendance/{id}", request);
        return response != null;
    }

    public async Task<AttendanceStatisticsDto?> GetAttendanceStatisticsAsync(int studentId, int? courseId = null)
    {
        var endpoint = $"api/v1.0/attendance/student/{studentId}/statistics";
        if (courseId.HasValue)
        {
            endpoint += $"?courseId={courseId}";
        }
        
        var response = await _apiService.GetWithResponseAsync<AttendanceStatisticsDto>(endpoint);
        return response?.Data;
    }
}
