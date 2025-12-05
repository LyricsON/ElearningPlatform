using System.Net.Http.Json;
using Elearning.Blazor.Models;

namespace Elearning.Blazor.Services;

public interface IEnrollmentsApiClient
{
    Task<List<EnrollmentDto>> GetEnrollmentsAsync(int? userId = null, int? courseId = null);
    Task<EnrollmentDto?> GetEnrollmentByIdAsync(int id);
    Task<bool> CreateEnrollmentAsync(CreateEnrollmentDto dto);
    Task<bool> UpdateEnrollmentAsync(int id, UpdateEnrollmentDto dto);
    Task<bool> DeleteEnrollmentAsync(int id);
}

public class EnrollmentsApiClient : IEnrollmentsApiClient
{
    private readonly HttpClient _httpClient;

    public EnrollmentsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EnrollmentDto>> GetEnrollmentsAsync(int? userId = null, int? courseId = null)
    {
        try
        {
            var url = "/api/enrollments";
            var query = new List<string>();
            if (userId.HasValue) query.Add($"userId={userId.Value}");
            if (courseId.HasValue) query.Add($"courseId={courseId.Value}");
            if (query.Count > 0)
            {
                url += "?" + string.Join("&", query);
            }

            var result = await _httpClient.GetFromJsonAsync<List<EnrollmentDto>>(url);
            return result ?? new List<EnrollmentDto>();
        }
        catch
        {
            return new List<EnrollmentDto>();
        }
    }

    public async Task<EnrollmentDto?> GetEnrollmentByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<EnrollmentDto>($"/api/enrollments/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CreateEnrollmentAsync(CreateEnrollmentDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/enrollments", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateEnrollmentAsync(int id, UpdateEnrollmentDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/enrollments/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteEnrollmentAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/enrollments/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
