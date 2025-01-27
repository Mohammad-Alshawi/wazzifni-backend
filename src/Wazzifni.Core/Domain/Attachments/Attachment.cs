using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.Attachments
{
    // Attachment model
    public class Attachment : FullAuditedEntity<long>
    {
        [StringLength(500)]
        public string Name { get; set; }

        public AttachmentType Type { get; set; }

        [Required]
        [StringLength(1000)]
        public string RelativePath { get; set; }
        public string LowResolutionPhotoRelativePath { get; set; }


        public long? RefId { get; set; }

        public AttachmentRefType RefType { get; set; }

        public string Size { get; set; }

        public static bool IsValidAttachmentRefType(byte type)
        {
            return Enum.IsDefined(typeof(AttachmentRefType), type);
        }
    }


}



