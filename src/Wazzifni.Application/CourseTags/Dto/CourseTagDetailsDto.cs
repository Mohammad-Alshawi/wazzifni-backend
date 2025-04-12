using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Wazzifni.CourseTags.Dto
{
    public class CourseTagDetailsDto : EntityDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationTime { get; set; }
        public List<CourseTagTranslationDto> Translations { get; set; }


    }
}
