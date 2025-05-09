using Abp.Application.Services.Dto;
using Wazzifni.Domain.Messages;
using Wazzifni.Messages.Dto;
using Wazzifni.Users.Dto;

namespace Wazzifni.Chats.Dto
{
    public class LiteChatDto : EntityDto<long>
    {
        public MessageLiteDto LastMessage { get; set; }
        public UserDto User { get; set; }
    }
}
