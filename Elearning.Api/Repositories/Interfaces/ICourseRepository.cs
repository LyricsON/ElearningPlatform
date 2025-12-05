using Elearning.Api.Models;

namespace Elearning.Api.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(int id);
    Task<Course?> GetDetailsAsync(int id);
    Task<IEnumerable<Course>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<Course>> GetByInstructorAsync(int instructorId);
    Task<Course> CreateAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(int id);
}
