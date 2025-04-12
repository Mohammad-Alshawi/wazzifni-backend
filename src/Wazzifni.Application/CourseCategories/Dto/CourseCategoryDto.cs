using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wazzifni.Countries.Dto;

namespace Wazzifni.CourseCategories.Dto
{
    public class CourseCategoryDto : EntityDto<int>
    {
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; }
        public CountryDto Country { get; set; }
        public List<CourseCategoryTranslationDto> Translations { get; set; }
    }
}
