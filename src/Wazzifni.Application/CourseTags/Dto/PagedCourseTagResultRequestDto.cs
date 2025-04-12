using Abp.Application.Services.Dto;

namespace Wazzifni.CourseTags.Dto
{
    public class PagedCourseTagResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

        public bool? IsActive { get; set; }
    }
}
