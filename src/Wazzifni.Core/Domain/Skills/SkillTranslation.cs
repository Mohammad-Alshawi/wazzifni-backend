using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Wazzifni.Domain.Skills
{
    public class SkillTranslation : FullAuditedEntity, IEntityTranslation<Skill>
    {
        public string Name { get; set; }
        public Skill Core { get; set; }
        public int CoreId { get; set; }
        public string Language { get; set; }
    }
}
