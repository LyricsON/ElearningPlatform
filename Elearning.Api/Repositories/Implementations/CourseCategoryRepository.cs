using Elearning.Api.Data;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Elearning.Api.Repositories.Implementations;

public class CourseCategoryRepository : ICourseCategoryRepository
{
    private readonly ElearningDbContext _context;

    public CourseCategoryRepository(ElearningDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseCategory>> GetAllAsync()
    {
        return await _context.CourseCategories
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<CourseCategory?> GetByIdAsync(int id)
    {
        return await _context.CourseCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<CourseCategory> CreateAsync(CourseCategory category)
    {
        _context.CourseCategories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task UpdateAsync(CourseCategory category)
    {
        if (!await _context.CourseCategories.AnyAsync(c => c.Id == category.Id))
            throw new KeyNotFoundException($"Category with id {category.Id} was not found.");

        _context.CourseCategories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.CourseCategories.FindAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Category with id {id} was not found.");

        _context.CourseCategories.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
