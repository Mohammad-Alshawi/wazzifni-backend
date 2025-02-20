using Abp.Application.Services.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.WorkPosts.Dto
{
    public class PagedWorkPostResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

        public int? CompanyId { get; set; }

        public WorkPostStatus? Status { get; set; }
        public WorkEngagement? WorkEngagement { get; set; }
        public WorkLevel? WorkLevel { get; set; }
        public EducationLevel? EducationLevel { get; set; }

        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public WorkAvailbility? WorkAvailbility { get; set; }

        public long? ProfileId { get; set; }

        public bool? IsFavorite { get; set; }
        public bool? IsIApply { get; set; }

        public WorkPlace? WorkPlace { get; set; }
        public int? MinExperience { get; set; }
        public int? MaxExperience { get; set; }
        public FilterOnTime? TimeFilter { get; set; }
    }
}
