using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Wazzifni.Authorization.Users;

namespace Wazzifni.Messages.Dto
{
    public class MessageDetailsDto : EntityDto<Guid>
    {
        public long? UserSenderId { get; set; }

        public long? UserReceiverId { get; set; }

        public string Content { get; set; }
    }
}
