using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using Wazzifni.Cities.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Companies.Dto
{
    public class LiteCompanyDto : EntityDto<int>
    {
        public string Name { get; set; }
        public string About { get; set; }
        public List<CompanyTranslationDto> Translations { get; set; }
        public string Address { get; set; }
        public LiteRegionCityDto City { get; set; }
        public SuperLiteUserDto User { get; set; }
        // public int Rate { get; set; }
        public bool IsActive { get; set; }
        public CompanyStatus Status { get; set; }

        public string JobType { get; set; }

        public DateTime? DateOfEstablishment { get; set; }
        public string ReasonRefuse { get; set; }
        public int? NumberOfEmployees { get; set; }
        public LiteAttachmentDto CompanyProfile { get; set; }

        public List<LiteAttachmentDto> Attachments { get; set; } = new List<LiteAttachmentDto>();


    }

}
