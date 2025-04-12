using Abp.Application.Services.Dto;
using System.Collections.Generic;
using Wazzifni.Countries.Dto;

namespace Wazzifni.CourseCategories.Dto
{
    public class LiteCourseCategoryDto : EntityDto<int>
    {

        public List<CourseCategoryTranslationDto> Translations { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public LiteAttachmentDto Attachment { get; set; }
    }
    public class LiteRegionCourseCategoryDto : EntityDto<int>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        //public LiteCourseCategoryDto CourseCategory { get; set; }
        //public List<RegionTranslationDto> Translations { get; set; }
    }


    public class LiteCourseCategory : EntityDto<int>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<CourseCategoryTranslationDto> Translations { get; set; }
    }

}
