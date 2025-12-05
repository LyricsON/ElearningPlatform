using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;

namespace Elearning.Api.Profiles;

public class QuizQuestionProfile : Profile
{
    public QuizQuestionProfile()
    {
        CreateMap<QuizQuestion, QuizQuestionDto>();
        CreateMap<CreateQuizQuestionDto, QuizQuestion>();
        CreateMap<UpdateQuizQuestionDto, QuizQuestion>();
    }
}
