using Elearning.Api.Models;

namespace Elearning.Api.Repositories.Interfaces;

public interface IQuizRepository
{
    Task<IEnumerable<Quiz>> GetAllAsync();
    Task<Quiz?> GetByIdAsync(int id);
    Task<Quiz?> GetWithQuestionsAsync(int id);
    Task<IEnumerable<Quiz>> GetByCourseAsync(int courseId);
    Task<Quiz> CreateAsync(Quiz quiz);
    Task UpdateAsync(Quiz quiz);
    Task DeleteAsync(int id);
}
