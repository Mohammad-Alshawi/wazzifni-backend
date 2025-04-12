using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using Wazzifni.Countries.Dto;

namespace Wazzifni.CourseCategories.Dto
{
    public class CourseCategoryDetailsDto : EntityDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationTime { get; set; }
        public List<CourseCategoryTranslationDto> Translations { get; set; }
        public LiteAttachmentDto Attachment { get; set; }

    }
}
