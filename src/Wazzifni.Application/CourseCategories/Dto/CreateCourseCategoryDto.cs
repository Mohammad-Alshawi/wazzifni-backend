using Abp.Runtime.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.CourseCategories.Dto
{
    public class CreateCourseCategoryDto : ICustomValidate
    {
        [Required]
        public List<CourseCategoryTranslationDto> Translations { get; set; }
        [Required]
        public long AttachmentId { get; set; }
        public virtual void AddValidationErrors(CustomValidationContext context)
        {
            if (Translations is null || Translations.Count < 2)
                context.Results.Add(new ValidationResult("Translations must contain at least two elements"));
           
        }
    }
}
