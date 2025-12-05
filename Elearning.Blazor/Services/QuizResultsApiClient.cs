using System.Net.Http.Json;
using Elearning.Blazor.Models;

namespace Elearning.Blazor.Services;

public interface IQuizResultsApiClient
{
    Task<List<QuizResultDto>> GetQuizResultsAsync(int? userId = null, int? quizId = null);
    Task<QuizResultDto?> GetQuizResultByIdAsync(int id);
    Task<bool> CreateQuizResultAsync(CreateQuizResultDto dto);
    Task<bool> DeleteQuizResultAsync(int id);
    Task<QuizScoreResponseDto?> SubmitQuizAsync(SubmitQuizDto dto);
}

public class QuizResultsApiClient : IQuizResultsApiClient
{
    private readonly HttpClient _httpClient;

    public QuizResultsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<QuizResultDto>> GetQuizResultsAsync(int? userId = null, int? quizId = null)
    {
        try
        {
            var url = "/api/quizresults";
            var query = new List<string>();
            if (userId.HasValue) query.Add($"userId={userId.Value}");
            if (quizId.HasValue) query.Add($"quizId={quizId.Value}");
            if (query.Count > 0)
            {
                url += "?" + string.Join("&", query);
            }

            var result = await _httpClient.GetFromJsonAsync<List<QuizResultDto>>(url);
            return result ?? new List<QuizResultDto>();
        }
        catch
        {
            return new List<QuizResultDto>();
        }
    }

    public async Task<QuizResultDto?> GetQuizResultByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<QuizResultDto>($"/api/quizresults/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CreateQuizResultAsync(CreateQuizResultDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/quizresults", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteQuizResultAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/quizresults/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<QuizScoreResponseDto?> SubmitQuizAsync(SubmitQuizDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/quizresults/submit", dto);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<QuizScoreResponseDto>();
        }
        catch
        {
            return null;
        }
    }
}
