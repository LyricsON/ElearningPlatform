using Elearning.Api.Data;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Elearning.Api.Repositories.Implementations;

public class QuizQuestionRepository : IQuizQuestionRepository
{
    private readonly ElearningDbContext _context;

    public QuizQuestionRepository(ElearningDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<QuizQuestion>> GetAllAsync()
    {
        return await _context.QuizQuestions
            .Include(q => q.Quiz)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<QuizQuestion?> GetByIdAsync(int id)
    {
        return await _context.QuizQuestions
            .Include(q => q.Quiz)
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<IEnumerable<QuizQuestion>> GetByQuizAsync(int quizId)
    {
        return await _context.QuizQuestions
            .Where(q => q.QuizId == quizId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<QuizQuestion> CreateAsync(QuizQuestion question)
    {
        _context.QuizQuestions.Add(question);
        await _context.SaveChangesAsync();
        return question;
    }

    public async Task UpdateAsync(QuizQuestion question)
    {
        if (!await _context.QuizQuestions.AnyAsync(q => q.Id == question.Id))
            throw new KeyNotFoundException($"Question with id {question.Id} was not found.");

        _context.QuizQuestions.Update(question);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.QuizQuestions.FindAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Question with id {id} was not found.");

        _context.QuizQuestions.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
