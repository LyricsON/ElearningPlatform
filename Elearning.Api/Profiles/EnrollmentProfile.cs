using AutoMapper;
using Elearning.Api.Dtos;
using Elearning.Api.Models;

namespace Elearning.Api.Profiles;

public class EnrollmentProfile : Profile
{
    public EnrollmentProfile()
    {
        CreateMap<Enrollment, EnrollmentDto>();
        CreateMap<CreateEnrollmentDto, Enrollment>();
        CreateMap<UpdateEnrollmentDto, Enrollment>();
    }
}
