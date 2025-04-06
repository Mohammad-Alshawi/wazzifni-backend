using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.WorkApplications;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.WorkPosts
{
    public class WorkPost : FullAuditedEntity<long>
    {
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }
        public WorkPostStatus Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public WorkEngagement WorkEngagement { get; set; }
        public WorkPlace WorkPlace { get; set; }

        public WorkLevel WorkLevel { get; set; }
        public EducationLevel EducationLevel { get; set; }

        [Precision(20, 5)]
        public decimal MinSalary { get; set; }

        [Precision(20, 5)]

        public decimal MaxSalary { get; set; }
        public int ExperienceYearsCount { get; set; }
        public int RequiredEmployeesCount { get; set; }
        public int ApplicantsCount { get; set; }
        public WorkAvailbility WorkAvailbility { get; set; } = WorkAvailbility.Available;

        public bool IsClosed { get; set; } = false;

        public DateTime? ClosedDate { get; set; }

        public virtual ICollection<WorkApplication> Applications { get; set; }

        public string Slug { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string ReasonRefuse { get; set; }



    }
}
