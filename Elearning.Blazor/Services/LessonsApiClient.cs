using System.Net.Http.Json;
using Elearning.Blazor.Models;

namespace Elearning.Blazor.Services;

public interface ILessonsApiClient
{
    Task<List<LessonDto>> GetLessonsAsync(int? courseId = null);
    Task<LessonDto?> GetLessonByIdAsync(int id);
    Task<(bool Success, string? Error)> CreateLessonAsync(CreateLessonDto dto);
    Task<bool> UpdateLessonAsync(int id, UpdateLessonDto dto);
    Task<(bool Success, string? Error)> DeleteLessonAsync(int id);
}

public class LessonsApiClient : ILessonsApiClient
{
    private readonly HttpClient _httpClient;

    public LessonsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<LessonDto>> GetLessonsAsync(int? courseId = null)
    {
        try
        {
            var url = "/api/lessons";
            if (courseId.HasValue)
            {
                url += $"?courseId={courseId.Value}";
            }

            var result = await _httpClient.GetFromJsonAsync<List<LessonDto>>(url);
            return result ?? new List<LessonDto>();
        }
        catch
        {
            return new List<LessonDto>();
        }
    }

    public async Task<LessonDto?> GetLessonByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<LessonDto>($"/api/lessons/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<(bool Success, string? Error)> CreateLessonAsync(CreateLessonDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/lessons", dto);
            if (response.IsSuccessStatusCode)
                return (true, null);
            
            var errorBody = await response.Content.ReadAsStringAsync();
            return (false, string.IsNullOrWhiteSpace(errorBody) 
                ? $"Erreur {response.StatusCode}" 
                : errorBody);
        }
        catch (Exception ex)
        {
            return (false, $"Exception: {ex.Message}");
        }
    }

    public async Task<bool> UpdateLessonAsync(int id, UpdateLessonDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/lessons/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<(bool Success, string? Error)> DeleteLessonAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/lessons/{id}");
            if (response.IsSuccessStatusCode)
                return (true, null);
            
            var errorBody = await response.Content.ReadAsStringAsync();
            return (false, string.IsNullOrWhiteSpace(errorBody) 
                ? $"Erreur {response.StatusCode}" 
                : errorBody);
        }
        catch (Exception ex)
        {
            return (false, $"Exception: {ex.Message}");
        }
    }
}
