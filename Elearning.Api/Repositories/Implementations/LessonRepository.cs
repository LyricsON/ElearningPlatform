using Elearning.Api.Data;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Elearning.Api.Repositories.Implementations;

public class LessonRepository : ILessonRepository
{
    private readonly ElearningDbContext _context;

    public LessonRepository(ElearningDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Lesson>> GetAllAsync()
    {
        return await _context.Lessons
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(int id)
    {
        return await _context.Lessons
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<Lesson>> GetByCourseAsync(int courseId)
    {
        return await _context.Lessons
            .Where(l => l.CourseId == courseId)
            .OrderBy(l => l.OrderNumber)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Lesson> CreateAsync(Lesson lesson)
    {
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
        return lesson;
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        if (!await _context.Lessons.AnyAsync(l => l.Id == lesson.Id))
            throw new KeyNotFoundException($"Lesson with id {lesson.Id} was not found.");

        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Lessons.FindAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Lesson with id {id} was not found.");

        _context.Lessons.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
