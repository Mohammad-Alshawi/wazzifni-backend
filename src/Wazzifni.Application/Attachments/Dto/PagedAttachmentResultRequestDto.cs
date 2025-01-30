using Abp.Application.Services.Dto;

namespace Wazzifni.Attachments.Dto
{

    public class PagedAttachmentResultRequestDto : PagedAndSortedResultRequestDto
    {

        public long RefId { get; set; }


        public byte RefType { get; set; }
    }
}