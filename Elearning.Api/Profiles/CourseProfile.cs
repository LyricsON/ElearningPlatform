using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;

namespace Elearning.Api.Profiles;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor != null ? $"{src.Instructor.FirstName} {src.Instructor.LastName}".Trim() : null));

        CreateMap<Course, CourseDetailsDto>()
            .IncludeBase<Course, CourseDto>()
            .ForMember(dest => dest.Lessons, opt => opt.MapFrom(src => src.Lessons.OrderBy(l => l.OrderNumber)))
            .ForMember(dest => dest.Quizzes, opt => opt.MapFrom(src => src.Quizzes));

        CreateMap<CreateCourseDto, Course>();
        CreateMap<UpdateCourseDto, Course>();
    }
}
