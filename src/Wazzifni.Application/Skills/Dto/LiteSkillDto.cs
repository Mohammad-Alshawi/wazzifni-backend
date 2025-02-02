using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace Wazzifni.Skills.Dto
{
    public class LiteSkillDto : EntityDto<int>
    {

        public List<SkillTranslationDto> Translations { get; set; }
        public string Name { get; set; }

        public bool IsActive { get; set; }

    }




}
