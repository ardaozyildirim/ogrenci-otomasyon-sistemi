using StudentManagementSystem.Frontend.Models.DTOs;

namespace StudentManagementSystem.Frontend.Services.Api;

public class GradeService
{
    private readonly ApiService _apiService;

    public GradeService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<GradeDto>?> GetStudentGradesAsync(int studentId)
    {
        var response = await _apiService.GetWithResponseAsync<List<GradeDto>>($"api/grades/student/{studentId}");
        return response?.Data;
    }

    public async Task<GradeDto?> AssignGradeAsync(AssignGradeRequest request)
    {
        var response = await _apiService.PostWithResponseAsync<GradeDto>("api/grades", request);
        return response?.Data;
    }

    public async Task<bool> UpdateGradeAsync(int id, UpdateGradeRequest request)
    {
        request.Id = id;
        var response = await _apiService.PutAsync<GradeDto>($"api/grades/{id}", request);
        return response != null;
    }
}
