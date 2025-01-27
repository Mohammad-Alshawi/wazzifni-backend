using Abp.Application.Services.Dto;
using System.Collections.Generic;
using Wazzifni.Countries.Dto;

namespace Wazzifni.Cities.Dto
{
    public class LiteCityDto : EntityDto<int>
    {

        public List<CityTranslationDto> Translations { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; }
        public CountryDto Country { get; set; }
        public List<LiteRegionCityDto> Regions { get; set; }
        public LiteAttachmentDto Attachment { get; set; }
    }
    public class LiteRegionCityDto : EntityDto<int>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        //public LiteCityDto City { get; set; }
        //public List<RegionTranslationDto> Translations { get; set; }
    }


    public class LiteCity : EntityDto<int>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<CityTranslationDto> Translations { get; set; }
    }

}
