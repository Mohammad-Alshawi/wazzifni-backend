using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Wazzifni.Domain.IndividualUserProfiles;

namespace Wazzifni.Domain.WorkExperiences
{
    public class WorkExperience : FullAuditedEntity<long>
    {
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentJob { get; set; }
        public string Description { get; set; }
        public long ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public virtual Profile Profile { get; set; }
    }
}
