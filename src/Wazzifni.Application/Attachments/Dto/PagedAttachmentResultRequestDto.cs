using Abp.Application.Services.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Attachments.Dto
{

    public class PagedAttachmentResultRequestDto : PagedAndSortedResultRequestDto
    {

        public long RefId { get; set; }


        public byte RefType { get; set; }

        public AttachmentRefType Type { get; set; }
    }
}