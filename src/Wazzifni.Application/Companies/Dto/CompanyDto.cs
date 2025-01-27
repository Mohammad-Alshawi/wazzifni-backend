﻿using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace Wazzifni.Domain.Companies.Dto
{
    public class CompanyDto : EntityDto<int>
    {
        public string About { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<CompanyTranslationDto> Translations { get; set; }
        public CompanyContactDto CompanyContact { get; set; }
        public GeneralRatingDto GeneralRating { get; set; }
        // public List<ReviewDetailsDto> Reviews { get; set; }
        public int NumberOfTransfers { get; set; }
        public string CommissionGroup { get; set; }

    }
}
