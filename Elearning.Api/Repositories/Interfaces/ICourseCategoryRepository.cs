using Elearning.Api.Models;

namespace Elearning.Api.Repositories.Interfaces;

public interface ICourseCategoryRepository
{
    Task<IEnumerable<CourseCategory>> GetAllAsync();
    Task<CourseCategory?> GetByIdAsync(int id);
    Task<CourseCategory> CreateAsync(CourseCategory category);
    Task UpdateAsync(CourseCategory category);
    Task DeleteAsync(int id);
}
