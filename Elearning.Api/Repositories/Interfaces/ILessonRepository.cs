using Elearning.Api.Models;

namespace Elearning.Api.Repositories.Interfaces;

public interface ILessonRepository
{
    Task<IEnumerable<Lesson>> GetAllAsync();
    Task<Lesson?> GetByIdAsync(int id);
    Task<IEnumerable<Lesson>> GetByCourseAsync(int courseId);
    Task<Lesson> CreateAsync(Lesson lesson);
    Task UpdateAsync(Lesson lesson);
    Task DeleteAsync(int id);
}
