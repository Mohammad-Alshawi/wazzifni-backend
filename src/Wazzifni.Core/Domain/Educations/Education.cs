using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Wazzifni.Domain.IndividualUserProfiles;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.Educations
{
    public class Education : FullAuditedEntity<long>
    {
        public EducationLevel Level { get; set; }
        public string InstitutionName { get; set; }
        public string FieldOfStudy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentlyStudying { get; set; }
        public string Description { get; set; }

        public long ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public virtual Profile Profile { get; set; }
        public long UserId { get; set; }

    }
}
