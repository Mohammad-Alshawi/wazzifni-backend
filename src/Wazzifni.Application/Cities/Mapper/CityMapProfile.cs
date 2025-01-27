using AutoMapper;
using Wazzifni.Cities.Dto;
using Wazzifni.Domain.Cities;

namespace Wazzifni.Cities.Mapper
{
    public class CityMapProfile : Profile
    {
        public CityMapProfile()
        {
            CreateMap<CreateCityDto, City>();
            CreateMap<CreateCityDto, CityDto>();
            CreateMap<CityDto, City>();
            CreateMap<UpdateCityDto, City>();
            CreateMap<LiteCity, City>();

        }
    }
}
