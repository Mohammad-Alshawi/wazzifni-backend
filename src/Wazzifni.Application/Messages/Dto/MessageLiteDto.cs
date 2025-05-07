using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Wazzifni.Companies.Dto;
using Wazzifni.Courses.Dto;
using Wazzifni.Trainees.Dto;

namespace Wazzifni.Messages.Dto
{
    public class MessageLiteDto : EntityDto<Guid>
    {
        public long UserSenderId { get; set; }
        public SuperLiteUserDto UserSender { get; set; }

        public long UserReceiverId { get; set; }
        public SuperLiteUserDto UserReceiver { get; set; }

        public string Content { get; set; }

        public bool ISent { get; set; }

        public bool IReceive { get; set; }

        public DateTime CreationTime { get; set; }

        public bool OwnerIsAdmin { get; set; }

    }
}
