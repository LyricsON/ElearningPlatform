using Elearning.Api.Dtos;

namespace Elearning.Api.Services.Interfaces;

public interface IEnrollmentService
{
    Task<IEnumerable<EnrollmentDto>> GetAllAsync();
    Task<EnrollmentDto?> GetByIdAsync(int id);
    Task<IEnumerable<EnrollmentDto>> GetByUserAsync(int userId);
    Task<IEnumerable<EnrollmentDto>> GetByCourseAsync(int courseId);
    Task<EnrollmentDto> CreateAsync(CreateEnrollmentDto dto);
    Task<bool> UpdateAsync(int id, UpdateEnrollmentDto dto);
    Task<bool> DeleteAsync(int id);
}
