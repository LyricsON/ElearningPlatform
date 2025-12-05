using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;

namespace Elearning.Api.Profiles;

public class QuizProfile : Profile
{
    public QuizProfile()
    {
        CreateMap<Quiz, QuizDto>();
        CreateMap<CreateQuizDto, Quiz>();
        CreateMap<UpdateQuizDto, Quiz>();
    }
}
