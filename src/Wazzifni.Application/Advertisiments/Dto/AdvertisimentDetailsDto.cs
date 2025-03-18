using Abp.Application.Services.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Advertisiments.Dto
{
    public class AdvertisimentDetailsDto : EntityDto
    {
        public LiteAttachmentDto Attachment { get; set; }
        public string? Link { get; set; }
        public long? WorkPostId { get; set; }
        public long? CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        public bool? IsForWorkPost { get; set; }

        public UserType Type { get; set; }

    }
}
