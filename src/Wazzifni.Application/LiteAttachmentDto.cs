using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;
using Wazzifni.Domain.Attachments;

namespace Wazzifni
{
    /// <summary>
    /// 
    /// </summary>
    [AutoMap(typeof(Attachment))]
    public class LiteAttachmentDto : EntityDto<long>
    {
        public LiteAttachmentDto()
        {
        }

        public LiteAttachmentDto(long id, string url, string lowResolutionPhotoUrl, string size, long? refId = null)
        {
            Id = id;
            Url = url;
            Size = size;
            LowResolutionPhotoUrl = lowResolutionPhotoUrl;
            RefId = refId;
        }

        public string Url { get; set; }
        public string LowResolutionPhotoUrl { get; set; }
        public string Size { get; set; }
        public long? RefId { get; set; }

        public string Name { get; set; }

    }

    public class EnumInfo
    {
        public string Name { get; set; }
        public List<EnumValue> Values { get; set; }
    }

    public class EnumValue
    {
        public string Name { get; set; }
        public byte Value { get; set; }
    }
}
