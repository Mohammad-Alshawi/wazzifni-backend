using System;
using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;
using Microsoft.EntityFrameworkCore;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.WorkPosts.Dto
{
    public class CreateWorkPostDto : ICustomValidate
    {
        public int? CompanyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public WorkEngagement WorkEngagement { get; set; }
        public WorkLevel WorkLevel { get; set; }
        public EducationLevel EducationLevel { get; set; }
        [Precision(20, 5)]
        public decimal MinSalary { get; set; }
        [Precision(20, 5)]
        public decimal MaxSalary { get; set; }
        public int ExperienceYearsCount { get; set; }
        public int RequiredEmployeesCount { get; set; }
        public WorkPlace WorkPlace { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (RequiredEmployeesCount < 1)
                context.Results.Add(new ValidationResult("RequiredEmployeesCount is required"));
        }
    }
}
