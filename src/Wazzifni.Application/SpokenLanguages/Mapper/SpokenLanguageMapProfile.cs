using AutoMapper;
using Wazzifni.Domain.SpokenLanguages;
using Wazzifni.SpokenLanguages.DTOs;

namespace Wazzifni.SpokenLanguages.Mapper;

public class SpokenLanguageMapProfile : Profile
{
    public SpokenLanguageMapProfile()
    {
        CreateMap<SpokenLanguage, SpokenLanguageDto>();
        CreateMap<CreateSpokenLanguageDto, SpokenLanguage>();
        CreateMap<UpdateSpokenLanguageDto, SpokenLanguage>();
        CreateMap<SpokenLanguage, SpokenLanguageDetailsDto>();
        CreateMap<SpokenLanguage, LiteSpokenLanguageDto>();

    }
}