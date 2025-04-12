using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Wazzifni.CourseTags.Dto;

namespace Wazzifni.CourseTags.Dto
{
    public class LiteCourseTagDto : EntityDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationTime { get; set; }
        public List<CourseTagTranslationDto> Translations { get; set; }
    }
}
