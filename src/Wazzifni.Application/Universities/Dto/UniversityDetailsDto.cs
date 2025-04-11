using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Wazzifni.Universities.Dto
{
    public class UniversityDetailsDto : EntityDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationTime { get; set; }
        public List<UniversityTranslationDto> Translations { get; set; }


    }
}
