using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using Wazzifni.Authorization.Users;

namespace Wazzifni.Domain.Feedbacks
{
    public class Feedback : FullAuditedEntity<long>
    {
        public long? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
