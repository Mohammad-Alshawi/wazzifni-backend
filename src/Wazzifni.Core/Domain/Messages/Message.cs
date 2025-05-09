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
    public class Message : FullAuditedEntity<Guid>
    {
        public long UserSenderId { get; set; }
        [ForeignKey(nameof(UserSenderId))]
        public virtual User UserSender { get; set; }

        public long? UserReceiverId { get; set; }
        [ForeignKey(nameof(UserReceiverId))]
        public virtual User? UserReceiver { get; set; }

        public bool OwnerIsAdmin { get; set; }
        public string Content { get; set; }

        public long? ChatId { get; set; }
        [ForeignKey(nameof(ChatId))]
        public virtual Chat? Chat { get; set; }
    }
}
