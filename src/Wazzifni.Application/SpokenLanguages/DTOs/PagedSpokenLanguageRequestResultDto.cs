using Abp.Application.Services.Dto;

namespace Wazzifni.SpokenLanguages.DTOs
{
    public class PagedSpokenLanguageRequestResultDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
    }
}
