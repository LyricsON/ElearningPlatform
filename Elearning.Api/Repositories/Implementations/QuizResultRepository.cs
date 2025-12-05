using Elearning.Api.Data;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Elearning.Api.Repositories.Implementations;

public class QuizResultRepository : IQuizResultRepository
{
    private readonly ElearningDbContext _context;

    public QuizResultRepository(ElearningDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<QuizResult>> GetAllAsync()
    {
        return await _context.QuizResults
            .Include(r => r.User)
            .Include(r => r.Quiz)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<QuizResult?> GetByIdAsync(int id)
    {
        return await _context.QuizResults
            .Include(r => r.User)
            .Include(r => r.Quiz)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<QuizResult>> GetByUserAsync(int userId)
    {
        return await _context.QuizResults
            .Include(r => r.Quiz)
            .Where(r => r.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<QuizResult>> GetByQuizAsync(int quizId)
    {
        return await _context.QuizResults
            .Include(r => r.User)
            .Where(r => r.QuizId == quizId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<QuizResult?> GetByUserAndQuizAsync(int userId, int quizId)
    {
        return await _context.QuizResults
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.UserId == userId && r.QuizId == quizId);
    }

    public async Task<QuizResult> CreateAsync(QuizResult result)
    {
        _context.QuizResults.Add(result);
        await _context.SaveChangesAsync();
        return result;
    }

    public async Task UpdateAsync(QuizResult result)
    {
        if (!await _context.QuizResults.AnyAsync(r => r.Id == result.Id))
            throw new KeyNotFoundException($"Quiz result with id {result.Id} was not found.");

        _context.QuizResults.Update(result);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.QuizResults.FindAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Quiz result with id {id} was not found.");

        _context.QuizResults.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
