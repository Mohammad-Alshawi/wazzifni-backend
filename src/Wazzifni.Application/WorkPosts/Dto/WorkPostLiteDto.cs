using Abp.Application.Services.Dto;
using System;
using Wazzifni.Companies.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.WorkPosts.Dto
{
    public class WorkPostLiteDto : EntityDto<long>
    {
        public int CompanyId { get; set; }
        public LiteCompanyDto Company { get; set; }
        public WorkPostStatus Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public WorkEngagement WorkEngagement { get; set; }
        public WorkLevel WorkLevel { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public int ExperienceYearsCount { get; set; }
        public int RequiredEmployeesCount { get; set; }
        public int ApplicantsCount { get; set; }
        public WorkAvailbility WorkAvailbility { get; set; }

        public DateTime CreationTime { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsIApply { get; set; }
        public WorkPlace WorkPlace { get; set; }

        public string Slug { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ReasonRefuse { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime? IsFeaturedAt { get; set; }

    }
}
