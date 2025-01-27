using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using Wazzifni.Domain.IndividualUserProfiles;

namespace Wazzifni.Domain.Skills
{
    public class Skill : FullAuditedEntity, IActiveEntity, IMultiLingualEntity<SkillTranslation>
    {
        public virtual ICollection<SkillTranslation> Translations { get; set; }

        public bool IsActive { set; get; }

        public virtual ICollection<Profile> Profiles { get; set; }

    }
}
