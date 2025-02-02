using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Wazzifni.Skills.Dto
{
    public class SkillDetailsDto : EntityDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationTime { get; set; }
        public List<SkillTranslationDto> Translations { get; set; }


    }
}
