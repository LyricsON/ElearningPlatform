using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Elearning.Api.Services.Interfaces;

namespace Elearning.Api.Services.Implementations;

public class LessonService : ILessonService
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public LessonService(ILessonRepository lessonRepository, ICourseRepository courseRepository, IMapper mapper)
    {
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LessonDto>> GetAllAsync()
    {
        var lessons = await _lessonRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<LessonDto>>(lessons);
    }

    public async Task<LessonDto?> GetByIdAsync(int id)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id);
        return lesson == null ? null : _mapper.Map<LessonDto>(lesson);
    }

    public async Task<IEnumerable<LessonDto>> GetByCourseAsync(int courseId)
    {
        var lessons = await _lessonRepository.GetByCourseAsync(courseId);
        return _mapper.Map<IEnumerable<LessonDto>>(lessons);
    }

    public async Task<LessonDto> CreateAsync(CreateLessonDto dto)
    {
        await EnsureCourseExists(dto.CourseId);

        var lesson = _mapper.Map<Lesson>(dto);
        var created = await _lessonRepository.CreateAsync(lesson);
        return _mapper.Map<LessonDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateLessonDto dto)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id);
        if (lesson == null)
            return false;

        _mapper.Map(dto, lesson);
        lesson.Id = id;
        await _lessonRepository.UpdateAsync(lesson);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _lessonRepository.DeleteAsync(id);
            return true;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }

    public async Task<bool> VerifyInstructorOwnership(int lessonId, int instructorId)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId);
        if (lesson == null)
            return false;

        var course = await _courseRepository.GetByIdAsync(lesson.CourseId);
        if (course == null)
            return false;

        return course.InstructorId == instructorId;
    }

    private async Task EnsureCourseExists(int courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            throw new InvalidOperationException($"Course with id {courseId} does not exist.");
    }
}
