using Elearning.Api.Dtos;

namespace Elearning.Api.Services.Interfaces;

public interface IQuizResultService
{
    Task<IEnumerable<QuizResultDto>> GetAllAsync();
    Task<QuizResultDto?> GetByIdAsync(int id);
    Task<IEnumerable<QuizResultDto>> GetByUserAsync(int userId);
    Task<IEnumerable<QuizResultDto>> GetByQuizAsync(int quizId);
    Task<QuizResultDto> CreateAsync(CreateQuizResultDto dto);
    Task<QuizScoreResponseDto> SubmitQuizAsync(SubmitQuizDto dto);
    Task<bool> DeleteAsync(int id);
}
