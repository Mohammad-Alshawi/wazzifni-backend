using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using Wazzifni.Domain.WorkPosts;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.Advertisiments
{
    public class Advertisiment : FullAuditedEntity
    {
        public string Link { get; set; }
        public long? WorkPostId { get; set; }
        [ForeignKey(nameof(WorkPostId))]
        public WorkPost WorkPost { get; set; }

        public UserType Type { get; set; }


    }
}
