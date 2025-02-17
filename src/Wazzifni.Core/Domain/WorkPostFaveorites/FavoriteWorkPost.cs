using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using Wazzifni.Domain.WorkPosts;

namespace Wazzifni.Domain.WorkPostFaveorites
{
    public class FavoriteWorkPost : FullAuditedEntity<long>
    {
        public long WorkPostId { get; set; }
        [ForeignKey(nameof(WorkPostId))]
        public virtual WorkPost WorkPost { get; set; }
    }
}
