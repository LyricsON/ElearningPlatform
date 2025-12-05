using System.Net.Http.Json;
using Elearning.Blazor.Models;

namespace Elearning.Blazor.Services;

public interface IQuizQuestionsApiClient
{
    Task<List<QuizQuestionDto>> GetQuestionsAsync(int? quizId = null);
    Task<QuizQuestionDto?> GetQuestionByIdAsync(int id);
    Task<bool> CreateQuestionAsync(CreateQuizQuestionDto dto);
    Task<bool> UpdateQuestionAsync(int id, UpdateQuizQuestionDto dto);
    Task<bool> DeleteQuestionAsync(int id);
}

public class QuizQuestionsApiClient : IQuizQuestionsApiClient
{
    private readonly HttpClient _httpClient;

    public QuizQuestionsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<QuizQuestionDto>> GetQuestionsAsync(int? quizId = null)
    {
        try
        {
            var url = "/api/quizquestions";
            if (quizId.HasValue)
            {
                url += $"?quizId={quizId.Value}";
            }

            var result = await _httpClient.GetFromJsonAsync<List<QuizQuestionDto>>(url);
            return result ?? new List<QuizQuestionDto>();
        }
        catch
        {
            return new List<QuizQuestionDto>();
        }
    }

    public async Task<QuizQuestionDto?> GetQuestionByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<QuizQuestionDto>($"/api/quizquestions/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CreateQuestionAsync(CreateQuizQuestionDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/quizquestions", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateQuestionAsync(int id, UpdateQuizQuestionDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/quizquestions/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteQuestionAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/quizquestions/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
