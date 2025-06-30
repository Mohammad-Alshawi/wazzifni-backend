using AutoMapper;
using Wazzifni.Domain.MobileApps;
using Wazzifni.Domain.MobileApps.Dtos;

namespace Wazzifni.MobileApps.Mapper
{
    public class MobileAppMapProfile : Profile
    {
        public MobileAppMapProfile()
        {
            CreateMap<CreateMobileAppDto, MobileApp>();
            CreateMap<MobileApp, LiteMobileAppDto>();
            CreateMap<MobileApp, MobileAppDetailsDto>();
            CreateMap<UpdateMobileAppDto, MobileApp>();


        }
    }
}
