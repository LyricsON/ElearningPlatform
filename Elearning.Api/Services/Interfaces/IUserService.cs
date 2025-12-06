using Elearning.Api.Dtos;

namespace Elearning.Api.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto?> GetByEmailAsync(string email);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task<bool> UpdateAsync(int id, UpdateUserDto dto);
    Task<bool> UpdateRoleAsync(int id, string role);
    Task<bool> DeleteAsync(int id);
}
