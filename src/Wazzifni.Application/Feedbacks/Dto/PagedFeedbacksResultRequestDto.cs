using Abp.Application.Services.Dto;

namespace Wazzifni.Feedbacks.Dto
{
    public class PagedFeedbacksResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
