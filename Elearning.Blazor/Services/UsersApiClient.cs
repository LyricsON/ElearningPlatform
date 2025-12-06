using System.Net.Http.Json;
using Elearning.Blazor.Models;

namespace Elearning.Blazor.Services;

public interface IUsersApiClient
{
    Task<List<UserDto>> GetUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<bool> CreateUserAsync(CreateUserDto dto);
    Task<bool> UpdateUserAsync(int id, UpdateUserDto dto);
    Task<bool> UpdateUserRoleAsync(int id, string role);
    Task<bool> DeleteUserAsync(int id);
}

public class UsersApiClient : IUsersApiClient
{
    private readonly HttpClient _httpClient;

    public UsersApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<UserDto>>("/api/users");
            return result ?? new List<UserDto>();
        }
        catch
        {
            return new List<UserDto>();
        }
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<UserDto>($"/api/users/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CreateUserAsync(CreateUserDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/users", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/users/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateUserRoleAsync(int id, string role)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/users/{id}/role", new { Role = role });
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/users/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
