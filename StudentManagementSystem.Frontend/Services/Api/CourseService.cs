using StudentManagementSystem.Frontend.Models.DTOs;

namespace StudentManagementSystem.Frontend.Services.Api;

public class CourseService
{
    private readonly ApiService _apiService;

    public CourseService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<CourseDto>?> GetAllCoursesAsync(int? pageNumber = null, int? pageSize = null, int? teacherId = null, string? department = null)
    {
        var queryParams = new List<string>();
        
        if (pageNumber.HasValue) queryParams.Add($"pageNumber={pageNumber}");
        if (pageSize.HasValue) queryParams.Add($"pageSize={pageSize}");
        if (teacherId.HasValue) queryParams.Add($"teacherId={teacherId}");
        if (!string.IsNullOrEmpty(department)) queryParams.Add($"department={Uri.EscapeDataString(department)}");
        
        var endpoint = "api/courses";
        if (queryParams.Any())
        {
            endpoint += "?" + string.Join("&", queryParams);
        }
        
        var response = await _apiService.GetWithResponseAsync<List<CourseDto>>(endpoint);
        return response?.Data;
    }

    public async Task<CourseDto?> GetCourseByIdAsync(int id)
    {
        var response = await _apiService.GetWithResponseAsync<CourseDto>($"api/courses/{id}");
        return response?.Data;
    }

    public async Task<CourseDto?> CreateCourseAsync(CreateCourseRequest request)
    {
        var response = await _apiService.PostWithResponseAsync<CourseDto>("api/courses", request);
        return response?.Data;
    }

    public async Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseRequest request)
    {
        request.Id = id;
        var response = await _apiService.PutAsync<CourseDto>($"api/courses/{id}", request);
        return response;
    }
}
