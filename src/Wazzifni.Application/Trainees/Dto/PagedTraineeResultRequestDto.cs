using Abp.Application.Services.Dto;

namespace Wazzifni.Trainees.Dto
{
    public class PagedTraineeResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? UniversityId { get; set; }

    }
}
