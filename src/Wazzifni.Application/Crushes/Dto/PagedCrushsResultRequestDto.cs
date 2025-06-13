using Abp.Application.Services.Dto;

namespace Wazzifni.Crushes.Dto
{
    public class PagedCrushsResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
