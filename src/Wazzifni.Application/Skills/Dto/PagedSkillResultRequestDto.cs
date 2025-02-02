using Abp.Application.Services.Dto;

namespace Wazzifni.Skills.Dto
{
    public class PagedSkillResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

        public bool? IsActive { get; set; }
    }
}
