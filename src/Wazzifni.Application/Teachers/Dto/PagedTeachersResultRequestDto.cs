using Abp.Application.Services.Dto;

namespace Wazzifni.Teachers.Dto
{
    public class PagedTeachersResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
