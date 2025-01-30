using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Wazzifni.Domain.Attachments;

namespace Wazzifni.Attachments.Dto
{

    [AutoMapTo(typeof(Attachment))]
    public class UploadMultiAttachmentInputDto : ICustomValidate
    {

        public byte RefType { get; set; }


        [Required(ErrorMessage = "Required")]
        public IFormFileCollection Files { get; set; }


        public void AddValidationErrors(CustomValidationContext context)
        {
            if (!Attachment.IsValidAttachmentRefType(RefType))
            {
            }
        }
    }
}
