using System.Net.Http.Json;
using Elearning.Blazor.Models;

namespace Elearning.Blazor.Services;

public interface ICoursesApiClient
{
    Task<List<CourseDto>> GetCoursesAsync(int? categoryId = null, int? instructorId = null);
    Task<CourseDetailsDto?> GetCourseByIdAsync(int id);
    Task<(bool Success, string? Error)> CreateCourseAsync(CreateCourseDto dto);
    Task<bool> UpdateCourseAsync(int id, UpdateCourseDto dto);
    Task<bool> DeleteCourseAsync(int id);
}

public class CoursesApiClient : ICoursesApiClient
{
    private readonly HttpClient _httpClient;

    public CoursesApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CourseDto>> GetCoursesAsync(int? categoryId = null, int? instructorId = null)
    {
        try
        {
            var url = "/api/courses";

            if (categoryId.HasValue || instructorId.HasValue)
            {
                var query = new List<string>();
                if (categoryId.HasValue) query.Add($"categoryId={categoryId.Value}");
                if (instructorId.HasValue) query.Add($"instructorId={instructorId.Value}");
                url += "?" + string.Join("&", query);
            }

            var result = await _httpClient.GetFromJsonAsync<List<CourseDto>>(url);
            return result ?? new List<CourseDto>();
        }
        catch
        {
            return new List<CourseDto>();
        }
    }

    public async Task<CourseDetailsDto?> GetCourseByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<CourseDetailsDto>($"/api/courses/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<(bool Success, string? Error)> CreateCourseAsync(CreateCourseDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/courses", dto);
            if (response.IsSuccessStatusCode)
                return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            return (false, string.IsNullOrWhiteSpace(body) ? response.StatusCode.ToString() : body);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<bool> UpdateCourseAsync(int id, UpdateCourseDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/courses/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteCourseAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/courses/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
