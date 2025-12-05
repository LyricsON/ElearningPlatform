using Elearning.Api.Data;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Elearning.Api.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly ElearningDbContext _context;

    public UserRepository(ElearningDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AppUser>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<AppUser?> GetByIdAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<AppUser?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<AppUser> CreateAsync(AppUser user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(AppUser user)
    {
        if (!await _context.Users.AnyAsync(u => u.Id == user.Id))
            throw new KeyNotFoundException($"User with id {user.Id} was not found.");

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Users.FindAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"User with id {id} was not found.");

        _context.Users.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
