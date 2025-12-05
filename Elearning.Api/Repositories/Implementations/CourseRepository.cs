using Elearning.Api.Data;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Elearning.Api.Repositories.Implementations;

public class CourseRepository : ICourseRepository
{
    private readonly ElearningDbContext _context;

    public CourseRepository(ElearningDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Instructor)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Instructor)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Course?> GetDetailsAsync(int id)
    {
        return await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Instructor)
            .Include(c => c.Lessons)
            .Include(c => c.Quizzes)
            .ThenInclude(q => q.Questions)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Course>> GetByCategoryAsync(int categoryId)
    {
        return await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Instructor)
            .Where(c => c.CategoryId == categoryId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetByInstructorAsync(int instructorId)
    {
        return await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Instructor)
            .Where(c => c.InstructorId == instructorId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Course> CreateAsync(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task UpdateAsync(Course course)
    {
        if (!await _context.Courses.AnyAsync(c => c.Id == course.Id))
            throw new KeyNotFoundException($"Course with id {course.Id} was not found.");

        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Courses.FindAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Course with id {id} was not found.");

        _context.Courses.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
