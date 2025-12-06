using Elearning.Api.Dtos;

namespace Elearning.Api.Services.Interfaces;

public interface IQuizQuestionService
{
    Task<IEnumerable<QuizQuestionDto>> GetAllAsync();
    Task<QuizQuestionDto?> GetByIdAsync(int id);
    Task<IEnumerable<QuizQuestionDto>> GetByQuizAsync(int quizId);
    Task<QuizQuestionDto> CreateAsync(CreateQuizQuestionDto dto);
    Task<bool> UpdateAsync(int id, UpdateQuizQuestionDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> VerifyInstructorOwnership(int questionId, int instructorId);
}
