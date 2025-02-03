using Abp.Application.Services.Dto;

namespace Wazzifni.Profiles.Dto
{
    public class PagedProfileResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

    }
}
