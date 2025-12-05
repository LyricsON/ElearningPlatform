using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Elearning.Api.Services.Interfaces;

namespace Elearning.Api.Services.Implementations;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public EnrollmentService(
        IEnrollmentRepository enrollmentRepository,
        IUserRepository userRepository,
        ICourseRepository courseRepository,
        IMapper mapper)
    {
        _enrollmentRepository = enrollmentRepository;
        _userRepository = userRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EnrollmentDto>> GetAllAsync()
    {
        var enrollments = await _enrollmentRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
    }

    public async Task<EnrollmentDto?> GetByIdAsync(int id)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);
        return enrollment == null ? null : _mapper.Map<EnrollmentDto>(enrollment);
    }

    public async Task<IEnumerable<EnrollmentDto>> GetByUserAsync(int userId)
    {
        var enrollments = await _enrollmentRepository.GetByUserAsync(userId);
        return _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
    }

    public async Task<IEnumerable<EnrollmentDto>> GetByCourseAsync(int courseId)
    {
        var enrollments = await _enrollmentRepository.GetByCourseAsync(courseId);
        return _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
    }

    public async Task<EnrollmentDto> CreateAsync(CreateEnrollmentDto dto)
    {
        await EnsureUserExists(dto.UserId);
        await EnsureCourseExists(dto.CourseId);

        var existing = await _enrollmentRepository.GetByUserAndCourseAsync(dto.UserId, dto.CourseId);
        if (existing != null)
            throw new InvalidOperationException("User is already enrolled in this course.");

        var enrollment = _mapper.Map<Enrollment>(dto);
        var created = await _enrollmentRepository.CreateAsync(enrollment);
        return _mapper.Map<EnrollmentDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateEnrollmentDto dto)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);
        if (enrollment == null)
            return false;

        if (dto.ProgressPercent < 0 || dto.ProgressPercent > 100)
            throw new InvalidOperationException("Progress percent must be between 0 and 100.");

        _mapper.Map(dto, enrollment);
        enrollment.Id = id;

        await _enrollmentRepository.UpdateAsync(enrollment);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _enrollmentRepository.DeleteAsync(id);
            return true;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }

    private async Task EnsureUserExists(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException($"User with id {userId} does not exist.");
    }

    private async Task EnsureCourseExists(int courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            throw new InvalidOperationException($"Course with id {courseId} does not exist.");
    }
}
