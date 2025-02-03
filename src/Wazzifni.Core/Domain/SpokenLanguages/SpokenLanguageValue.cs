using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using Wazzifni.Domain.IndividualUserProfiles;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.SpokenLanguages
{
    public class SpokenLanguageValue : FullAuditedEntity<long>
    {
        public long ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public virtual Profile Profile { get; set; }

        public int SpokenLanguageId { get; set; }
        [ForeignKey(nameof(SpokenLanguageId))]
        public virtual SpokenLanguage SpokenLanguage { get; set; }

        public SpokenLanguageLevel OralLevel { get; set; }

        public SpokenLanguageLevel WritingLevel { get; set; }

        public bool IsNative { get; set; }

    }
}
