using Abp.Application.Services.Dto;

namespace Wazzifni.Universities.Dto
{
    public class PagedUniversityResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

        public bool? IsActive { get; set; }
    }
}
