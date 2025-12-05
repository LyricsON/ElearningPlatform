using Elearning.Api.Data;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Elearning.Api.Repositories.Implementations;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly ElearningDbContext _context;

    public EnrollmentRepository(ElearningDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Enrollment>> GetAllAsync()
    {
        return await _context.Enrollments
            .Include(e => e.User)
            .Include(e => e.Course)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Enrollment?> GetByIdAsync(int id)
    {
        return await _context.Enrollments
            .Include(e => e.User)
            .Include(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Enrollment>> GetByUserAsync(int userId)
    {
        return await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Enrollment>> GetByCourseAsync(int courseId)
    {
        return await _context.Enrollments
            .Include(e => e.User)
            .Where(e => e.CourseId == courseId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Enrollment?> GetByUserAndCourseAsync(int userId, int courseId)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);
    }

    public async Task<Enrollment> CreateAsync(Enrollment enrollment)
    {
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();
        return enrollment;
    }

    public async Task UpdateAsync(Enrollment enrollment)
    {
        if (!await _context.Enrollments.AnyAsync(e => e.Id == enrollment.Id))
            throw new KeyNotFoundException($"Enrollment with id {enrollment.Id} was not found.");

        _context.Enrollments.Update(enrollment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Enrollments.FindAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Enrollment with id {id} was not found.");

        _context.Enrollments.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
