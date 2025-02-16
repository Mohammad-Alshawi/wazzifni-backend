using Abp.Application.Services.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.WorkApplications.Dto
{
    public class PagedWorkApplicationResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

        public long? WorkPostId { get; set; }

        public WorkApplicationStatus? Status { get; set; }

        public int? CompanyId { get; set; }

        public long? ProfileId { get; set; }

    }
}
