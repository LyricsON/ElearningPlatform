using Elearning.Api.Models;

namespace Elearning.Api.Repositories.Interfaces;

public interface IQuizQuestionRepository
{
    Task<IEnumerable<QuizQuestion>> GetAllAsync();
    Task<QuizQuestion?> GetByIdAsync(int id);
    Task<IEnumerable<QuizQuestion>> GetByQuizAsync(int quizId);
    Task<QuizQuestion> CreateAsync(QuizQuestion question);
    Task UpdateAsync(QuizQuestion question);
    Task DeleteAsync(int id);
}
