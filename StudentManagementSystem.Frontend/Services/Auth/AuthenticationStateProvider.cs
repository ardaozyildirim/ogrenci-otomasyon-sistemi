using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.Security.Claims;

namespace StudentManagementSystem.Frontend.Services.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private bool _isInitialized = false;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // İlk yüklemede her zaman anonymous döndür
        if (!_isInitialized)
        {
            _isInitialized = true;
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        try
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var userRole = await _localStorage.GetItemAsync<string>("userRole");
            var user = await _localStorage.GetItemAsync<StudentManagementSystem.Frontend.Models.DTOs.UserDto>("user");

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user?.Id.ToString() ?? ""),
                new(ClaimTypes.Name, $"{user?.FirstName} {user?.LastName}"),
                new(ClaimTypes.Email, user?.Email ?? ""),
                new(ClaimTypes.Role, userRole ?? "")
            };

            var identity = new ClaimsIdentity(claims, "jwt");
            var principal = new ClaimsPrincipal(identity);

            return new AuthenticationState(principal);
        }
        catch
        {
            // Herhangi bir hata durumunda anonymous user döndür
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task MarkUserAsAuthenticatedAsync(string token, string role, StudentManagementSystem.Frontend.Models.DTOs.UserDto user)
    {
        await _localStorage.SetItemAsync("authToken", token);
        await _localStorage.SetItemAsync("userRole", role);
        await _localStorage.SetItemAsync("user", user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(claims, "jwt");
        var principal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public async Task MarkUserAsLoggedOutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("userRole");
        await _localStorage.RemoveItemAsync("user");

        var identity = new ClaimsIdentity();
        var principal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }
}
