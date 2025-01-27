﻿using Abp.Runtime.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.Countries.Dto
{
    public class CreateCountryDto : ICustomValidate
    {
        [Required]
        public List<CountryTranslationDto> Translations { get; set; }
        [Required]
        [StringLength(5)]
        public string DialCode { get; set; }

        public virtual void AddValidationErrors(CustomValidationContext context)
        {
            if (Translations is null || Translations.Count < 2)
                context.Results.Add(new ValidationResult("Translations must contain at least two elements"));
            if (DialCode.Length < 1)
                context.Results.Add(new ValidationResult("DialCode is required"));
        }
    }
}
