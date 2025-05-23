﻿using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.Universities.Dto
{
    public class UniversityDto : EntityDto<int>
    {
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<UniversityTranslationDto> Translations { get; set; }
    }
}
