using Elearning.Api.Dtos;

namespace Elearning.Api.Services.Interfaces;

public interface ICourseCategoryService
{
    Task<IEnumerable<CourseCategoryDto>> GetAllAsync();
    Task<CourseCategoryDto?> GetByIdAsync(int id);
    Task<CourseCategoryDto> CreateAsync(CreateCourseCategoryDto dto);
    Task<bool> UpdateAsync(int id, UpdateCourseCategoryDto dto);
    Task<bool> DeleteAsync(int id);
}
