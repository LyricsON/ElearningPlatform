using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Elearning.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Elearning.Api.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<AppUser> _passwordHasher;

    public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher<AppUser> passwordHasher)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var existing = await _userRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
            throw new InvalidOperationException("A user with this email already exists.");

        ValidateRole(dto.Role);

        var user = new AppUser
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Role = dto.Role
        };

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        }

        var created = await _userRepository.CreateAsync(user);
        return _mapper.Map<UserDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return false;

        var duplicate = await _userRepository.GetByEmailAsync(dto.Email);
        if (duplicate != null && duplicate.Id != id)
            throw new InvalidOperationException("A user with this email already exists.");

        ValidateRole(dto.Role);

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.Role = dto.Role;

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        }

        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<bool> UpdateRoleAsync(int id, string role)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return false;

        ValidateRole(role);

        user.Role = role;
        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _userRepository.DeleteAsync(id);
            return true;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }

    private static void ValidateRole(string role)
    {
        if (role != "Student" && role != "Instructor" && role != "Admin")
            throw new InvalidOperationException("Invalid role. Must be Student, Instructor, or Admin.");
    }
}
