using StudentManagementSystem.Frontend.Models.DTOs;

namespace StudentManagementSystem.Frontend.Services.Api;

public class TeacherService
{
    private readonly ApiService _apiService;

    public TeacherService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<TeacherDto>?> GetAllTeachersAsync(int? pageNumber = null, int? pageSize = null, string? department = null)
    {
        var queryParams = new List<string>();
        
        if (pageNumber.HasValue) queryParams.Add($"pageNumber={pageNumber}");
        if (pageSize.HasValue) queryParams.Add($"pageSize={pageSize}");
        if (!string.IsNullOrEmpty(department)) queryParams.Add($"department={Uri.EscapeDataString(department)}");
        
        var endpoint = "api/teachers";
        if (queryParams.Any())
        {
            endpoint += "?" + string.Join("&", queryParams);
        }
        
        var response = await _apiService.GetWithResponseAsync<List<TeacherDto>>(endpoint);
        return response?.Data;
    }

    public async Task<TeacherDto?> GetTeacherByIdAsync(int id)
    {
        var response = await _apiService.GetWithResponseAsync<TeacherDto>($"api/teachers/{id}");
        return response?.Data;
    }

    public async Task<TeacherDto?> CreateTeacherAsync(CreateTeacherRequest request)
    {
        var response = await _apiService.PostWithResponseAsync<TeacherDto>("api/teachers", request);
        return response?.Data;
    }

    public async Task<TeacherDto?> UpdateTeacherAsync(int id, UpdateTeacherRequest request)
    {
        request.Id = id;
        var response = await _apiService.PutAsync<TeacherDto>($"api/teachers/{id}", request);
        return response;
    }

    public async Task<bool> DeleteTeacherAsync(int id)
    {
        return await _apiService.DeleteAsync($"api/teachers/{id}");
    }
}
