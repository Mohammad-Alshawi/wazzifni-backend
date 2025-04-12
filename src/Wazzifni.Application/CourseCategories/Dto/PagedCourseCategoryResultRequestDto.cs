using Abp.Application.Services.Dto;

namespace Wazzifni.CourseCategories.Dto
{
    public class PagedCourseCategoryResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

        public bool? isActive { get; set; }
    }
}
