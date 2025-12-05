using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;

namespace Elearning.Api.Profiles;

public class LessonProfile : Profile
{
    public LessonProfile()
    {
        CreateMap<Lesson, LessonDto>();
        CreateMap<CreateLessonDto, Lesson>();
        CreateMap<UpdateLessonDto, Lesson>();
    }
}
