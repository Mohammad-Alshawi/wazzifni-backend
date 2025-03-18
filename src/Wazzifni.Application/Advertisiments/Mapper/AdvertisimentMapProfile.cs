using AutoMapper;
using Wazzifni.Advertisiments.Dto;
using Wazzifni.Domain.Advertisiments;
using Wazzifni.Domain.Attachments;

namespace Wazzifni.Advertisiments.Mapper
{
    /// <summary>
    /// PostsMapProfile
    /// </summary>
    public class AdvertisimentMapProfile : Profile
    {
        /// <summary>
        ///  Posts Map Profile 
        /// </summary>
        public AdvertisimentMapProfile()
        {
            CreateMap<Advertisiment, UpdateAdvertisimentDto>();
            CreateMap<CreateAdvertisimentDto, Advertisiment>();
            CreateMap<UpdateAdvertisimentDto, Advertisiment>();
            CreateMap<Advertisiment, CreateAdvertisimentDto>();
            CreateMap<Advertisiment, LiteAdvertisimentDto>();
            CreateMap<Advertisiment, AdvertisimentDetailsDto>();
            CreateMap<Attachment, string>().ConvertUsing(source => source.RelativePath ?? string.Empty);
            // CreateMap<CreateAdvertisimentPositionDto, AdvertisimentPosition>();
            // CreateMap<AdvertisimentPosition, AdvertisimentPositionDto>();
            // CreateMap<AddAdvertisimentPositionDto, AdvertisimentPosition>();
        }



    }
}
