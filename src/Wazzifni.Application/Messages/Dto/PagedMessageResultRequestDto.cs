using Abp.Application.Services.Dto;

namespace Wazzifni.Messages.Dto
{
    public class PagedMessageResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

        public long? UserSenderId { get; set; }

        public long? UserReceiverId { get; set; }


        public bool? MessagesISent { get; set; }
        public bool? MessagesIReceived { get; set; }

        public bool? MessagesReceiveAndSent { get; set; }

        public long? ChatId { get; set; }




    }
}
