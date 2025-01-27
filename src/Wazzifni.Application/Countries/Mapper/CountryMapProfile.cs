using AutoMapper;
using Wazzifni.Countries.Dto;

namespace Wazzifni.Countries.Mapper
{
    public class CountryMapProfile : Profile
    {
        public CountryMapProfile()
        {
            CreateMap<CreateCountryDto, Country>();
            CreateMap<CreateCountryDto, CountryDto>();
            CreateMap<CountryDto, Country>();
            CreateMap<Country, UpdateCountryDto>();
            CreateMap<UpdateCountryDto, Country>();
        }
    }
}
