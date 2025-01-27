using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.Countries.Dto
{
    public class CountryDetailsDto : EntityDto
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        // public List<LiteCityDto> Cities { get; set; }

        public DateTime CreationTime { get; set; }
        [Required]
        [StringLength(5)]
        public string DialCode { get; set; }
        public List<CountryTranslationDto> Translations { get; set; }
    }
}
