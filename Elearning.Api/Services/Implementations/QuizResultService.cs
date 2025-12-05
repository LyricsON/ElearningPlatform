using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Elearning.Api.Services.Interfaces;

namespace Elearning.Api.Services.Implementations;

public class QuizResultService : IQuizResultService
{
    private readonly IQuizResultRepository _resultRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public QuizResultService(
        IQuizResultRepository resultRepository,
        IQuizRepository quizRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _resultRepository = resultRepository;
        _quizRepository = quizRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<QuizResultDto>> GetAllAsync()
    {
        var results = await _resultRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<QuizResultDto>>(results);
    }

    public async Task<QuizResultDto?> GetByIdAsync(int id)
    {
        var result = await _resultRepository.GetByIdAsync(id);
        return result == null ? null : _mapper.Map<QuizResultDto>(result);
    }

    public async Task<IEnumerable<QuizResultDto>> GetByUserAsync(int userId)
    {
        var results = await _resultRepository.GetByUserAsync(userId);
        return _mapper.Map<IEnumerable<QuizResultDto>>(results);
    }

    public async Task<IEnumerable<QuizResultDto>> GetByQuizAsync(int quizId)
    {
        var results = await _resultRepository.GetByQuizAsync(quizId);
        return _mapper.Map<IEnumerable<QuizResultDto>>(results);
    }

    public async Task<QuizResultDto> CreateAsync(CreateQuizResultDto dto)
    {
        await EnsureUserExists(dto.UserId);
        await EnsureQuizExists(dto.QuizId);

        var existing = await _resultRepository.GetByUserAndQuizAsync(dto.UserId, dto.QuizId);
        if (existing != null)
            throw new InvalidOperationException("A result for this user and quiz already exists. Use submit to update it.");

        var result = _mapper.Map<QuizResult>(dto);
        result.TakenAt = DateTime.UtcNow;

        var created = await _resultRepository.CreateAsync(result);
        return _mapper.Map<QuizResultDto>(created);
    }

    public async Task<QuizScoreResponseDto> SubmitQuizAsync(SubmitQuizDto dto)
    {
        await EnsureUserExists(dto.UserId);

        var quiz = await _quizRepository.GetWithQuestionsAsync(dto.QuizId);
        if (quiz == null)
            throw new KeyNotFoundException($"Quiz with id {dto.QuizId} was not found.");

        if (quiz.Questions.Count == 0)
            throw new InvalidOperationException("Quiz has no questions.");

        int correct = 0;
        foreach (var question in quiz.Questions)
        {
            var answer = dto.Answers.FirstOrDefault(a => a.QuestionId == question.Id);
            if (answer == null)
                continue;

            if (string.Equals(answer.SelectedOption, question.CorrectAnswer, StringComparison.OrdinalIgnoreCase))
                correct++;
        }

        int totalQuestions = quiz.Questions.Count;
        int score = (int)Math.Round((double)correct * 100 / totalQuestions);

        var existing = await _resultRepository.GetByUserAndQuizAsync(dto.UserId, dto.QuizId);
        if (existing == null)
        {
            var result = new QuizResult
            {
                UserId = dto.UserId,
                QuizId = dto.QuizId,
                Score = score,
                TakenAt = DateTime.UtcNow
            };

            await _resultRepository.CreateAsync(result);
        }
        else
        {
            existing.Score = score;
            existing.TakenAt = DateTime.UtcNow;
            await _resultRepository.UpdateAsync(existing);
        }

        return new QuizScoreResponseDto
        {
            QuizId = dto.QuizId,
            UserId = dto.UserId,
            Score = score,
            TotalQuestions = totalQuestions,
            CorrectAnswers = correct
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _resultRepository.DeleteAsync(id);
            return true;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }

    private async Task EnsureQuizExists(int quizId)
    {
        var quiz = await _quizRepository.GetByIdAsync(quizId);
        if (quiz == null)
            throw new InvalidOperationException($"Quiz with id {quizId} does not exist.");
    }

    private async Task EnsureUserExists(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException($"User with id {userId} does not exist.");
    }
}
