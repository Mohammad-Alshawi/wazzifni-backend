using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Wazzifni.Domain.IndividualUserProfiles;

namespace Wazzifni.Awards
{
    public class Award : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public long ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public virtual Profile Profile { get; set; }
    }
}
