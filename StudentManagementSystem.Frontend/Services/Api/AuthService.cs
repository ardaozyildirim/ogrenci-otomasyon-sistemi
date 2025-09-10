using StudentManagementSystem.Frontend.Models.DTOs;
using Blazored.LocalStorage;

namespace StudentManagementSystem.Frontend.Services.Api;

public class AuthService
{
    private readonly ApiService _apiService;
    private readonly ILocalStorageService _localStorage;

    public AuthService(ApiService apiService, ILocalStorageService localStorage)
    {
        _apiService = apiService;
        _localStorage = localStorage;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _apiService.PostWithResponseAsync<AuthResponse>("api/auth/login", request);
        
        if (response?.Success == true && response.Data != null)
        {
            await _localStorage.SetItemAsync("authToken", response.Data.Token);
            await _localStorage.SetItemAsync("user", response.Data.User);
            await _localStorage.SetItemAsync("userRole", response.Data.Role);
        }
        
        return response?.Data;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var response = await _apiService.PostWithResponseAsync<AuthResponse>("api/auth/register", request);
        
        if (response?.Success == true && response.Data != null)
        {
            await _localStorage.SetItemAsync("authToken", response.Data.Token);
            await _localStorage.SetItemAsync("user", response.Data.User);
            await _localStorage.SetItemAsync("userRole", response.Data.Role);
        }
        
        return response?.Data;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("user");
        await _localStorage.RemoveItemAsync("userRole");
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        return !string.IsNullOrEmpty(token);
    }

    public async Task<string?> GetUserRoleAsync()
    {
        return await _localStorage.GetItemAsync<string>("userRole");
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        return await _localStorage.GetItemAsync<UserDto>("user");
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>("authToken");
    }
}
