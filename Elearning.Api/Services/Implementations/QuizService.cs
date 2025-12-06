using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Elearning.Api.Services.Interfaces;

namespace Elearning.Api.Services.Implementations;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public QuizService(IQuizRepository quizRepository, ICourseRepository courseRepository, IMapper mapper)
    {
        _quizRepository = quizRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<QuizDto>> GetAllAsync()
    {
        var quizzes = await _quizRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<QuizDto>>(quizzes);
    }

    public async Task<QuizDto?> GetByIdAsync(int id)
    {
        var quiz = await _quizRepository.GetByIdAsync(id);
        return quiz == null ? null : _mapper.Map<QuizDto>(quiz);
    }

    public async Task<IEnumerable<QuizDto>> GetByCourseAsync(int courseId)
    {
        var quizzes = await _quizRepository.GetByCourseAsync(courseId);
        return _mapper.Map<IEnumerable<QuizDto>>(quizzes);
    }

    public async Task<QuizDto> CreateAsync(CreateQuizDto dto)
    {
        await EnsureCourseExists(dto.CourseId);

        var quiz = _mapper.Map<Quiz>(dto);
        var created = await _quizRepository.CreateAsync(quiz);
        return _mapper.Map<QuizDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateQuizDto dto)
    {
        var quiz = await _quizRepository.GetByIdAsync(id);
        if (quiz == null)
            return false;

        _mapper.Map(dto, quiz);
        quiz.Id = id;
        await _quizRepository.UpdateAsync(quiz);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _quizRepository.DeleteAsync(id);
            return true;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }

    public async Task<bool> VerifyInstructorOwnership(int quizId, int instructorId)
    {
        var quiz = await _quizRepository.GetByIdAsync(quizId);
        if (quiz == null)
            return false;

        var course = await _courseRepository.GetByIdAsync(quiz.CourseId);
        if (course == null)
            return false;

        return course.InstructorId == instructorId;
    }

    private async Task EnsureCourseExists(int courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            throw new InvalidOperationException($"Course with id {courseId} does not exist.");
    }
}
