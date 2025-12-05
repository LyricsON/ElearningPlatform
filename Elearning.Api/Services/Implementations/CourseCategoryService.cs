using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;
using Elearning.Api.Repositories.Interfaces;
using Elearning.Api.Services.Interfaces;

namespace Elearning.Api.Services.Implementations;

public class CourseCategoryService : ICourseCategoryService
{
    private readonly ICourseCategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CourseCategoryService(ICourseCategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CourseCategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CourseCategoryDto>>(categories);
    }

    public async Task<CourseCategoryDto?> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category == null ? null : _mapper.Map<CourseCategoryDto>(category);
    }

    public async Task<CourseCategoryDto> CreateAsync(CreateCourseCategoryDto dto)
    {
        var category = _mapper.Map<CourseCategory>(dto);
        var created = await _categoryRepository.CreateAsync(category);
        return _mapper.Map<CourseCategoryDto>(created);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCourseCategoryDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            return false;

        _mapper.Map(dto, category);
        category.Id = id;
        await _categoryRepository.UpdateAsync(category);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _categoryRepository.DeleteAsync(id);
            return true;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }
}
