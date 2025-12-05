using System.Net.Http.Json;
using Elearning.Blazor.Models;

namespace Elearning.Blazor.Services;

public interface ICategoriesApiClient
{
    Task<List<CourseCategoryDto>> GetCategoriesAsync();
    Task<CourseCategoryDto?> GetCategoryByIdAsync(int id);
    Task<bool> CreateCategoryAsync(CreateCourseCategoryDto dto);
    Task<bool> UpdateCategoryAsync(int id, UpdateCourseCategoryDto dto);
    Task<bool> DeleteCategoryAsync(int id);
}

public class CategoriesApiClient : ICategoriesApiClient
{
    private readonly HttpClient _httpClient;

    public CategoriesApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CourseCategoryDto>> GetCategoriesAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<CourseCategoryDto>>("/api/coursecategories");
            return result ?? new List<CourseCategoryDto>();
        }
        catch
        {
            return new List<CourseCategoryDto>();
        }
    }

    public async Task<CourseCategoryDto?> GetCategoryByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<CourseCategoryDto>($"/api/coursecategories/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CreateCategoryAsync(CreateCourseCategoryDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/coursecategories", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateCategoryAsync(int id, UpdateCourseCategoryDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/coursecategories/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/coursecategories/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
