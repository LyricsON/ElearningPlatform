using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Elearning.Api.Services.Interfaces;

namespace Elearning.Api.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICourseCategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CourseService(
        ICourseRepository courseRepository,
        IUserRepository userRepository,
        ICourseCategoryRepository categoryRepository,
        IMapper mapper)
    {
        _courseRepository = courseRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CourseDto>> GetAllAsync()
    {
        var courses = await _courseRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    public async Task<CourseDto?> GetByIdAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        return course == null ? null : _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDetailsDto?> GetDetailsAsync(int id)
    {
        var course = await _courseRepository.GetDetailsAsync(id);
        return course == null ? null : _mapper.Map<CourseDetailsDto>(course);
    }

    public async Task<IEnumerable<CourseDto>> GetByCategoryAsync(int categoryId)
    {
        var courses = await _courseRepository.GetByCategoryAsync(categoryId);
        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    public async Task<IEnumerable<CourseDto>> GetByInstructorAsync(int instructorId)
    {
        var courses = await _courseRepository.GetByInstructorAsync(instructorId);
        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
    {
        await EnsureInstructorExists(dto.InstructorId);
        if (dto.CategoryId.HasValue)
            await EnsureCategoryExists(dto.CategoryId.Value);

        var course = _mapper.Map<Course>(dto);
        var created = await _courseRepository.CreateAsync(course);

        // re-load with includes for mapping convenience
        var detailed = await _courseRepository.GetByIdAsync(created.Id) ?? created;
        return _mapper.Map<CourseDto>(detailed);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCourseDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
            return false;

        // Verify instructor ownership (instructors can only update their own courses)
        if (course.InstructorId != dto.InstructorId)
            throw new InvalidOperationException("You can only update your own courses.");

        await EnsureInstructorExists(dto.InstructorId);
        if (dto.CategoryId.HasValue)
            await EnsureCategoryExists(dto.CategoryId.Value);

        _mapper.Map(dto, course);
        course.Id = id;

        await _courseRepository.UpdateAsync(course);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _courseRepository.DeleteAsync(id);
            return true;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }

    public async Task<bool> VerifyInstructorOwnership(int courseId, int instructorId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            return false;

        return course.InstructorId == instructorId;
    }

    private async Task EnsureInstructorExists(int instructorId)
    {
        var instructor = await _userRepository.GetByIdAsync(instructorId);
        if (instructor == null)
            throw new InvalidOperationException($"Instructor with id {instructorId} does not exist.");

        var role = instructor.Role?.Trim();
        if (!string.Equals(role, "Instructor", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Only users with Instructor or Admin role can create or manage courses.");
    }

    private async Task EnsureCategoryExists(int categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
            throw new InvalidOperationException($"Category with id {categoryId} does not exist.");
    }
}
