using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using Wazzifni.Countries.Dto;

namespace Wazzifni.Cities.Dto
{
    public class CityDetailsDto : EntityDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationTime { get; set; }
        public CountryDto Country { get; set; }
        //public List<LiteRegionDto> Regions { get; set; }
        public List<CityTranslationDto> Translations { get; set; }
        public LiteAttachmentDto Attachment { get; set; }

    }
}
