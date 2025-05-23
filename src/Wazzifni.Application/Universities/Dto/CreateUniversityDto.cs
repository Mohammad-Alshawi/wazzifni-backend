﻿using Abp.Runtime.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.Universities.Dto
{
    public class CreateUniversityDto : ICustomValidate
    {
        [Required]
        public List<UniversityTranslationDto> Translations { get; set; }

        public virtual void AddValidationErrors(CustomValidationContext context)
        {
            if (Translations is null || Translations.Count < 2)
                context.Results.Add(new ValidationResult("Translations must contain at least two elements"));

        }
    }
}
