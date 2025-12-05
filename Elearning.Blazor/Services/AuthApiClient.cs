using System.Net.Http.Headers;
using System.Net.Http.Json;
using Elearning.Blazor.Models;

namespace Elearning.Blazor.Services;

public interface IAuthApiClient
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
    Task<bool> RegisterAsync(RegisterRequestDto dto);
    Task LogoutAsync();
}

public class AuthApiClient : IAuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", dto);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public Task LogoutAsync()
    {
        // If the API supports logout, call it here. For now, just return completed.
        return Task.CompletedTask;
    }
}
