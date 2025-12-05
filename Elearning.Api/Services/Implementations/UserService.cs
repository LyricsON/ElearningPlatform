using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Elearning.Api.Services.Interfaces;

namespace Elearning.Api.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
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

        var user = _mapper.Map<AppUser>(dto);
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

        _mapper.Map(dto, user);
        user.Id = id;

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
}
