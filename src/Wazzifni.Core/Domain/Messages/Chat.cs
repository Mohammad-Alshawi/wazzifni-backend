using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Wazzifni.Authorization.Users;

namespace Wazzifni.Domain.Messages
{
    public class Chat : FullAuditedEntity<long>
    {
        public ICollection<Message> Messages { get; set; }

        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
