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
            var response = await _httpClient.GetAsync("/api/users");
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<UserDto>>();
                return result ?? new List<UserDto>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("You are not authorized to access this resource. Please login as an admin.");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to fetch users. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (UnauthorizedAccessException)
        {
            throw; // Re-throw auth exceptions
        }
        catch (HttpRequestException)
        {
            throw; // Re-throw HTTP exceptions
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred while fetching users: {ex.Message}", ex);
        }
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/users/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserDto>();
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user {id}: {ex.Message}");
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
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to update user role: {response.StatusCode} - {error}");
            }
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user role: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/users/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to delete user: {response.StatusCode} - {error}");
            }
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting user: {ex.Message}");
            return false;
        }
    }
}
