using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Wazzifni.Domain.Attachments;

namespace Wazzifni.Attachments.Dto
{
    /// <summary>
    /// UploadAttachmentInputDto
    /// </summary>
    [AutoMapTo(typeof(Attachment))]
    public class UploadAttachmentInputDto : ICustomValidate
    {
        /// <summary>
        ///  Profile = 1,       
        /// </summary>
        public byte RefType { get; set; }

        [Required(ErrorMessage = "Required")]
        public IFormFile File { get; set; }


        public void AddValidationErrors(CustomValidationContext context)
        {
            if (!Attachment.IsValidAttachmentRefType(RefType))
            {
                // context.Results.Add(new ValidationResult(L("InvalidAttachmentRefType"));
            }
        }
    }
}