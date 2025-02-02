using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.Skills.Dto
{
    public class SkillDto : EntityDto<int>
    {
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<SkillTranslationDto> Translations { get; set; }
    }
}
