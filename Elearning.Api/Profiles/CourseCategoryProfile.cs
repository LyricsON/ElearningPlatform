using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;

namespace Elearning.Api.Profiles;

public class CourseCategoryProfile : Profile
{
    public CourseCategoryProfile()
    {
        CreateMap<CourseCategory, CourseCategoryDto>();
        CreateMap<CreateCourseCategoryDto, CourseCategory>();
        CreateMap<UpdateCourseCategoryDto, CourseCategory>();
    }
}
