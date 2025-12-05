using Elearning.Api.Models;

namespace Elearning.Api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<AppUser>> GetAllAsync();
    Task<AppUser?> GetByIdAsync(int id);
    Task<AppUser?> GetByEmailAsync(string email);
    Task<AppUser> CreateAsync(AppUser user);
    Task UpdateAsync(AppUser user);
    Task DeleteAsync(int id);
}
