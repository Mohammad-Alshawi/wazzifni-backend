using Abp.Application.Services.Dto;
using System;
using Wazzifni.Companies.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.WorkPosts.Dto
{
    public class WorkPostDetailsDto : EntityDto<long>
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

    }
}
