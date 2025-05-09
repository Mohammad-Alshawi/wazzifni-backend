using Abp.Application.Services.Dto;

namespace Wazzifni.Chats.Dto
{
    public class PagedChatsResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? UserId { get; set; }


        internal bool IsAdmin { get; set; }
    }
}
