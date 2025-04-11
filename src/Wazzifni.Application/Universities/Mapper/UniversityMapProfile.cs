using AutoMapper;
using Wazzifni.Universities.Dto;

using University = Wazzifni.Domain.Universities.University;


namespace Wazzifni.Universitys.Mapper
{
    public class UniversityMapProfile : Profile
    {
        public UniversityMapProfile()
        {
            CreateMap<CreateUniversityDto, University>();
            CreateMap<CreateUniversityDto, UniversityDto>();
            CreateMap<UniversityDto, University>();
            CreateMap<UpdateUniversityDto, University>();

        }
    }
}
