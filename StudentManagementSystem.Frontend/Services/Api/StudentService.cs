using StudentManagementSystem.Frontend.Models.DTOs;

namespace StudentManagementSystem.Frontend.Services.Api;

public class StudentService
{
    private readonly ApiService _apiService;

    public StudentService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<StudentDto>?> GetAllStudentsAsync(int? pageNumber = null, int? pageSize = null, string? department = null, int? grade = null)
    {
        var queryParams = new List<string>();
        
        if (pageNumber.HasValue) queryParams.Add($"pageNumber={pageNumber}");
        if (pageSize.HasValue) queryParams.Add($"pageSize={pageSize}");
        if (!string.IsNullOrEmpty(department)) queryParams.Add($"department={Uri.EscapeDataString(department)}");
        if (grade.HasValue) queryParams.Add($"grade={grade}");
        
        var endpoint = "api/v1.0/students";
        if (queryParams.Any())
        {
            endpoint += "?" + string.Join("&", queryParams);
        }
        
        var response = await _apiService.GetWithResponseAsync<List<StudentDto>>(endpoint);
        return response?.Data;
    }

    public async Task<StudentDto?> GetStudentByIdAsync(int id)
    {
        var response = await _apiService.GetWithResponseAsync<StudentDto>($"api/v1.0/students/{id}");
        return response?.Data;
    }

    public async Task<StudentDto?> CreateStudentAsync(CreateStudentRequest request)
    {
        var response = await _apiService.PostWithResponseAsync<StudentDto>("api/v1.0/students", request);
        return response?.Data;
    }

    public async Task<StudentDto?> UpdateStudentAsync(int id, UpdateStudentRequest request)
    {
        request.Id = id;
        var response = await _apiService.PutAsync<StudentDto>($"api/v1.0/students/{id}", request);
        return response;
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        return await _apiService.DeleteAsync($"api/v1.0/students/{id}");
    }
}
