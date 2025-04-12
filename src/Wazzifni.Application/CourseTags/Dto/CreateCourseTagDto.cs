using Abp.Runtime.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.CourseTags.Dto
{
    public class CreateCourseTagDto : ICustomValidate
    {
        [Required]
        public List<CourseTagTranslationDto> Translations { get; set; }

        public virtual void AddValidationErrors(CustomValidationContext context)
        {
            if (Translations is null || Translations.Count < 2)
                context.Results.Add(new ValidationResult("Translations must contain at least two elements"));

        }
    }
}
