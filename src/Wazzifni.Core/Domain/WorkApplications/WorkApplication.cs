using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using Wazzifni.Domain.IndividualUserProfiles;
using Wazzifni.Domain.WorkPosts;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.WorkApplications
{
    public class WorkApplication : FullAuditedEntity<long>
    {
        public long ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public virtual Profile Profile { get; set; }
        public long WorkPostId { get; set; }
        [ForeignKey(nameof(WorkPostId))]
        public virtual WorkPost WorkPost { get; set; }
        public string Description { get; set; }
        public WorkApplicationStatus Status { get; set; }
        public string RejectReason { get; set; }

        public bool DeletedByCompany { get; set; }
    }
}
