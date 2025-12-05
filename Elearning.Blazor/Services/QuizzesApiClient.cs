using System.Net.Http.Json;
using Elearning.Blazor.Models;

namespace Elearning.Blazor.Services;

public interface IQuizzesApiClient
{
    Task<List<QuizDto>> GetQuizzesAsync(int? courseId = null);
    Task<QuizDto?> GetQuizByIdAsync(int id);
    Task<bool> CreateQuizAsync(CreateQuizDto dto);
    Task<bool> UpdateQuizAsync(int id, UpdateQuizDto dto);
    Task<bool> DeleteQuizAsync(int id);
}

public class QuizzesApiClient : IQuizzesApiClient
{
    private readonly HttpClient _httpClient;

    public QuizzesApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<QuizDto>> GetQuizzesAsync(int? courseId = null)
    {
        try
        {
            var url = "/api/quizzes";
            if (courseId.HasValue)
            {
                url += $"?courseId={courseId.Value}";
            }

            var result = await _httpClient.GetFromJsonAsync<List<QuizDto>>(url);
            return result ?? new List<QuizDto>();
        }
        catch
        {
            return new List<QuizDto>();
        }
    }

    public async Task<QuizDto?> GetQuizByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<QuizDto>($"/api/quizzes/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CreateQuizAsync(CreateQuizDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/quizzes", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateQuizAsync(int id, UpdateQuizDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/quizzes/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteQuizAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/quizzes/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
