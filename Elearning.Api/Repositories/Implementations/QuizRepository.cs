using Elearning.Api.Data;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Elearning.Api.Repositories.Implementations;

public class QuizRepository : IQuizRepository
{
    private readonly ElearningDbContext _context;

    public QuizRepository(ElearningDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Quiz>> GetAllAsync()
    {
        return await _context.Quizzes
            .Include(q => q.Course)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Quiz?> GetByIdAsync(int id)
    {
        return await _context.Quizzes
            .Include(q => q.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<Quiz?> GetWithQuestionsAsync(int id)
    {
        return await _context.Quizzes
            .Include(q => q.Course)
            .Include(q => q.Questions)
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<IEnumerable<Quiz>> GetByCourseAsync(int courseId)
    {
        return await _context.Quizzes
            .Include(q => q.Course)
            .Where(q => q.CourseId == courseId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Quiz> CreateAsync(Quiz quiz)
    {
        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();
        return quiz;
    }

    public async Task UpdateAsync(Quiz quiz)
    {
        if (!await _context.Quizzes.AnyAsync(q => q.Id == quiz.Id))
            throw new KeyNotFoundException($"Quiz with id {quiz.Id} was not found.");

        _context.Quizzes.Update(quiz);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Quizzes.FindAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Quiz with id {id} was not found.");

        _context.Quizzes.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
