using Elearning.Api.Models;

namespace Elearning.Api.Repositories.Interfaces;

public interface IQuizResultRepository
{
    Task<IEnumerable<QuizResult>> GetAllAsync();
    Task<QuizResult?> GetByIdAsync(int id);
    Task<IEnumerable<QuizResult>> GetByUserAsync(int userId);
    Task<IEnumerable<QuizResult>> GetByQuizAsync(int quizId);
    Task<QuizResult?> GetByUserAndQuizAsync(int userId, int quizId);
    Task<QuizResult> CreateAsync(QuizResult result);
    Task UpdateAsync(QuizResult result);
    Task DeleteAsync(int id);
}
