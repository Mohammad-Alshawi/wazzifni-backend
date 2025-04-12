using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.CourseTags.Dto
{
    public class CourseTagDto : EntityDto<int>
    {
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<CourseTagTranslationDto> Translations { get; set; }
    }
}
