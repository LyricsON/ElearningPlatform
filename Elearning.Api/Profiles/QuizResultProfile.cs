using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;

namespace Elearning.Api.Profiles;

public class QuizResultProfile : Profile
{
    public QuizResultProfile()
    {
        CreateMap<QuizResult, QuizResultDto>();
        CreateMap<CreateQuizResultDto, QuizResult>();
    }
}
