using System.Net.Http.Headers;
using System.Text.Json;
using Blazored.LocalStorage;
using StudentManagementSystem.Frontend.Models.DTOs;

namespace StudentManagementSystem.Frontend.Services.Api;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task SetAuthTokenAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        await SetAuthTokenAsync();
        var response = await _httpClient.GetAsync(endpoint);
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
        
        return default;
    }

    public async Task<ApiResponse<T>?> GetWithResponseAsync<T>(string endpoint)
    {
        await SetAuthTokenAsync();
        var response = await _httpClient.GetAsync(endpoint);
        var content = await response.Content.ReadAsStringAsync();
        
        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<ApiResponse<T>>(content, _jsonOptions);
        }
        
        return JsonSerializer.Deserialize<ApiResponse<T>>(content, _jsonOptions);
    }

    public async Task<T?> PostAsync<T>(string endpoint, object data)
    {
        await SetAuthTokenAsync();
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(endpoint, content);
        
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
        }
        
        return default;
    }

    public async Task<ApiResponse<T>?> PostWithResponseAsync<T>(string endpoint, object data)
    {
        await SetAuthTokenAsync();
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(endpoint, content);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<ApiResponse<T>>(responseContent, _jsonOptions);
    }

    public async Task<T?> PutAsync<T>(string endpoint, object data)
    {
        await SetAuthTokenAsync();
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync(endpoint, content);
        
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
        }
        
        return default;
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        await SetAuthTokenAsync();
        var response = await _httpClient.DeleteAsync(endpoint);
        return response.IsSuccessStatusCode;
    }

    public async Task<PagedResponse<T>?> GetPagedAsync<T>(string endpoint)
    {
        await SetAuthTokenAsync();
        var response = await _httpClient.GetAsync(endpoint);
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PagedResponse<T>>(content, _jsonOptions);
        }
        
        return default;
    }
}
