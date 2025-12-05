using Elearning.Api.Dtos;

namespace Elearning.Api.Services.Interfaces;

public interface ILessonService
{
    Task<IEnumerable<LessonDto>> GetAllAsync();
    Task<LessonDto?> GetByIdAsync(int id);
    Task<IEnumerable<LessonDto>> GetByCourseAsync(int courseId);
    Task<LessonDto> CreateAsync(CreateLessonDto dto);
    Task<bool> UpdateAsync(int id, UpdateLessonDto dto);
    Task<bool> DeleteAsync(int id);
}
