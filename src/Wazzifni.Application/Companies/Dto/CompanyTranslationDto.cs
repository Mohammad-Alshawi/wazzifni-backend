﻿using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Wazzifni.Domain.Companies;

namespace Wazzifni.Companies.Dto
{
    [AutoMap(typeof(CompanyTranslation))]
    public class CompanyTranslationDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string About { get; set; }
        public string Address { get; set; }
        public string Language { get; set; }

    }
}
