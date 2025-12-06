using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Elearning.Api.Services.Interfaces;

namespace Elearning.Api.Services.Implementations;

public class QuizQuestionService : IQuizQuestionService
{
    private readonly IQuizQuestionRepository _questionRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public QuizQuestionService(
        IQuizQuestionRepository questionRepository, 
        IQuizRepository quizRepository,
        ICourseRepository courseRepository,
        IMapper mapper)
    {
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<QuizQuestionDto>> GetAllAsync()
    {
        var questions = await _questionRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<QuizQuestionDto>>(questions);
    }

    public async Task<QuizQuestionDto?> GetByIdAsync(int id)
    {
        var question = await _questionRepository.GetByIdAsync(id);
        return question == null ? null : _mapper.Map<QuizQuestionDto>(question);
    }

    public async Task<IEnumerable<QuizQuestionDto>> GetByQuizAsync(int quizId)
    {
        var questions = await _questionRepository.GetByQuizAsync(quizId);
        return _mapper.Map<IEnumerable<QuizQuestionDto>>(questions);
    }

    public async Task<QuizQuestionDto> CreateAsync(CreateQuizQuestionDto dto)
    {
        await EnsureQuizExists(dto.QuizId);

        var question = _mapper.Map<QuizQuestion>(dto);
        var created = await _questionRepository.CreateAsync(question);
        return _mapper.Map<QuizQuestionDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateQuizQuestionDto dto)
    {
        var question = await _questionRepository.GetByIdAsync(id);
        if (question == null)
            return false;

        _mapper.Map(dto, question);
        question.Id = id;
        await _questionRepository.UpdateAsync(question);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _questionRepository.DeleteAsync(id);
            return true;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }

    public async Task<bool> VerifyInstructorOwnership(int questionId, int instructorId)
    {
        var question = await _questionRepository.GetByIdAsync(questionId);
        if (question == null)
            return false;

        var quiz = await _quizRepository.GetByIdAsync(question.QuizId);
        if (quiz == null)
            return false;

        var course = await _courseRepository.GetByIdAsync(quiz.CourseId);
        if (course == null)
            return false;

        return course.InstructorId == instructorId;
    }

    private async Task EnsureQuizExists(int quizId)
    {
        var quiz = await _quizRepository.GetByIdAsync(quizId);
        if (quiz == null)
            throw new InvalidOperationException($"Quiz with id {quizId} does not exist.");
    }
}
