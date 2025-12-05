using Elearning.Api.Dtos;

namespace Elearning.Api.Services.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllAsync();
    Task<CourseDto?> GetByIdAsync(int id);
    Task<CourseDetailsDto?> GetDetailsAsync(int id);
    Task<IEnumerable<CourseDto>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<CourseDto>> GetByInstructorAsync(int instructorId);
    Task<CourseDto> CreateAsync(CreateCourseDto dto);
    Task<bool> UpdateAsync(int id, UpdateCourseDto dto);
    Task<bool> DeleteAsync(int id);
}
