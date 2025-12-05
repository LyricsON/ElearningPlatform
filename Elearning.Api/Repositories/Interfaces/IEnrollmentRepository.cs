using Elearning.Api.Models;

namespace Elearning.Api.Repositories.Interfaces;

public interface IEnrollmentRepository
{
    Task<IEnumerable<Enrollment>> GetAllAsync();
    Task<Enrollment?> GetByIdAsync(int id);
    Task<IEnumerable<Enrollment>> GetByUserAsync(int userId);
    Task<IEnumerable<Enrollment>> GetByCourseAsync(int courseId);
    Task<Enrollment?> GetByUserAndCourseAsync(int userId, int courseId);
    Task<Enrollment> CreateAsync(Enrollment enrollment);
    Task UpdateAsync(Enrollment enrollment);
    Task DeleteAsync(int id);
}
