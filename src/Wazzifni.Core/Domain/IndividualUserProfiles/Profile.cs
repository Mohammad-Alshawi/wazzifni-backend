using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Wazzifni.Authorization.Users;
using Wazzifni.Awards;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.Educations;
using Wazzifni.Domain.Skills;
using Wazzifni.Domain.SpokenLanguages;
using Wazzifni.Domain.WorkApplications;
using Wazzifni.Domain.WorkExperiences;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.IndividualUserProfiles
{
    public class Profile : FullAuditedEntity<long>
    {
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public int CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; }
        public string About { get; set; }
        public virtual ICollection<WorkExperience> WorkExperiences { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public virtual ICollection<Award> Awards { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }

        public virtual ICollection<SpokenLanguageValue> SpokenLanguages { get; set; }

        public virtual ICollection<WorkApplication> Applications { get; set; }

        public Gender? Gender { get; set; } = null;

    }
}
