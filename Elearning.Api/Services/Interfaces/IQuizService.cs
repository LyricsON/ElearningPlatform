using Elearning.Api.Dtos;

namespace Elearning.Api.Services.Interfaces;

public interface IQuizService
{
    Task<IEnumerable<QuizDto>> GetAllAsync();
    Task<QuizDto?> GetByIdAsync(int id);
    Task<IEnumerable<QuizDto>> GetByCourseAsync(int courseId);
    Task<QuizDto> CreateAsync(CreateQuizDto dto);
    Task<bool> UpdateAsync(int id, UpdateQuizDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> VerifyInstructorOwnership(int quizId, int instructorId);
}
